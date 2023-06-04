using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Establishing connection to the SQLite Db.
string? connString = builder.Configuration.GetConnectionString("SqliteConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("api/todo", async (AppDbContext context) =>
{
	List<ToDo> items = await context.ToDos.ToListAsync();

	return Results.Ok(items);
});

app.MapPost("api/todo", async (AppDbContext context, ToDo toDo) =>
{
	await context.ToDos.AddAsync(toDo);

	await context.SaveChangesAsync();

	return Results.Created($"api/todo/{toDo.Id}", toDo);
});

app.MapPut("api/todo/{id}", async (AppDbContext context, int id, ToDo toDo) =>
{
	ToDo? toDoModel = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id);

	if(toDoModel == null)
		return Results.NotFound();

	// Manually mapping the new values to update the model:
	toDoModel.ToDoName = toDo.ToDoName;

	await context.SaveChangesAsync();

	return Results.NoContent();
});

app.MapDelete("api/todo/{id}", async (AppDbContext context, int id) =>
{
	ToDo? toDoModel = await context.ToDos.FirstOrDefaultAsync(t => t.Id == id);

	if (toDoModel == null)
		return Results.NotFound();

	context.ToDos.Remove(toDoModel);
	
	await context.SaveChangesAsync();

	return Results.NoContent();
});

app.Run();
