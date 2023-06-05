using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
	public class ToDo : INotifyPropertyChanged
	{
		private int _id;

		public int Id
		{
			get => _id;
			set 
			{ 
				// We are checking this just to make sure that our event is triggered only when the value is changed.
				if(_id == value) 
					return;

				_id = value;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
			}
		}

		private string _toDoName;

		public string ToDoName
		{
			get => _toDoName;
			set 
			{
				if(_toDoName == value) 
					return;

				_toDoName = value;

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToDoName)));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}
}
