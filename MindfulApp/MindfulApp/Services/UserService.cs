using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MindfulApp.Models;

namespace MindfulApp.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:5265/api/Users";

        public UserService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(baseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<User>>(response) ?? new List<User>();
            }
            catch { return new List<User>(); }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(json);
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{baseUrl}/{user.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"{baseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }
    }
}
