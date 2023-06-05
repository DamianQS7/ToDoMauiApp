using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.DataAccess
{
	public interface IRestDataService
	{
		Task<List<ToDo>> GetAllToDosAsync();
		Task AddToDoAsync(ToDo toDo);
		Task DeleteToDoAsync(int id);
		Task UpdateToDoAsync(ToDo toDo);
	}
}
