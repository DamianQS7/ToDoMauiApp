using System.Diagnostics;
using ToDoApp.DataAccess;
using ToDoApp.Models;

namespace ToDoApp.Pages;

[QueryProperty(nameof(ToDo), "ToDo")]
public partial class ManageToDoPage : ContentPage
{
	private readonly IRestDataService _dataService;
	private bool _isNew;
	private ToDo _toDo;

	public ToDo ToDo
	{
		get => _toDo; 
		set 
		{
			_isNew = IsNew(value);
			_toDo = value;
			OnPropertyChanged();
		}
	}

	public ManageToDoPage(IRestDataService dataService)
	{
		InitializeComponent();

		_dataService = dataService;

		BindingContext = this;
	}

	bool IsNew(ToDo toDo) => toDo.Id == 0;

	async void OnCancelBtnClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
	
	async void OnDeleteBtnClicked(object sender, EventArgs e)
	{
		await _dataService.DeleteToDoAsync(ToDo.Id);
		await Shell.Current.GoToAsync("..");
	}

	async void OnSaveBtnClicked(object sender, EventArgs e)
	{
		if (_isNew)
		{
			Debug.WriteLine("---> Add new item");
			await _dataService.AddToDoAsync(ToDo);
		}
		else
		{
			Debug.WriteLine("---> Update an item");
			await _dataService.UpdateToDoAsync(ToDo);
		}

		await Shell.Current.GoToAsync("..");
	}
}