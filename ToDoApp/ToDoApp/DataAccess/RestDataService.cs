using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.DataAccess
{
	public class RestDataService : IRestDataService
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseAddress;
		private readonly string _url;
		private readonly JsonSerializerOptions _jsonSerializeOptions;

		public RestDataService()
        {
			_httpClient = new HttpClient();
			_baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5209" : "https://localhost:7209";
			_url = $"{_baseAddress}/api";

			_jsonSerializeOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

			};
        }

        public Task AddToDoAsync(ToDo toDo)
		{
			throw new NotImplementedException();
		}

		public Task DeleteToDoAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<ToDo>> GetAllToDosAsync()
		{
			List<ToDo> toDos = new List<ToDo>();

			if(Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
			{
				Debug.WriteLine("No Internet Access :(");
				return toDos;
			}

			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/todo");

				if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();

					toDos = JsonSerializer.Deserialize<List<ToDo>>(responseContent, _jsonSerializeOptions);
				}
				else
				{
					Debug.WriteLine("Non Http 2xx Response");
				}

				return toDos;
			}
			catch(Exception ex)
			{
				Debug.WriteLine($"Exception: {ex.Message}");
			}

			return toDos;
		}

		public Task UpdateToDoAsync(ToDo toDo)
		{
			throw new NotImplementedException();
		}
	}
}
