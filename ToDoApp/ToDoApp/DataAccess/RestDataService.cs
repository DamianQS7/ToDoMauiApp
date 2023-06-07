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
			_baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5095" : "http://localhost:5095";
			_url = $"{_baseAddress}/api";

			_jsonSerializeOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
        }

        public async Task AddToDoAsync(ToDo toDo)
		{
			if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
			{
				Debug.WriteLine("No Internet Access :(");
				return;
			}

			try
			{
				string toDoJson = JsonSerializer.Serialize<ToDo>(toDo, _jsonSerializeOptions);
				StringContent content = new StringContent(toDoJson, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/todo", content);

				if (response.IsSuccessStatusCode)
					Debug.WriteLine("ToDo Successfully created!");
				else
					Debug.WriteLine("Non Http 2xx Response");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception: {ex.Message}");
			}

			return;
		}

		public async Task DeleteToDoAsync(int id)
		{
			if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
			{
				Debug.WriteLine("No Internet Access :(");
				return;
			}

			try
			{
				HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/todo/{id}");

				if (response.IsSuccessStatusCode)
					Debug.WriteLine("ToDo Successfully created!");
				else
					Debug.WriteLine("Non Http 2xx Response");
			}
			catch(Exception ex)
			{
				Debug.WriteLine($"Exception: {ex.Message}");
			}

			return;
		}

		public async Task<List<ToDo>> GetAllToDosAsync()
		{
			List<ToDo> toDos = new List<ToDo>();

			if(Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
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
					Debug.WriteLine("Items successfully fetched");
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

		public async Task UpdateToDoAsync(ToDo toDo)
		{
			if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
			{
				Debug.WriteLine("No Internet Access :(");
				return;
			}

			try
			{
				string toDoJson = JsonSerializer.Serialize<ToDo>(toDo, _jsonSerializeOptions);
				StringContent content = new StringContent(toDoJson, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _httpClient.PutAsync($"{_url}/todo/{toDo.Id}", content);

				if (response.IsSuccessStatusCode)
					Debug.WriteLine("ToDo Successfully updated!");
				else
					Debug.WriteLine("Non Http 2xx Response");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception: {ex.Message}");
			}

			return;
		}
	}
}
