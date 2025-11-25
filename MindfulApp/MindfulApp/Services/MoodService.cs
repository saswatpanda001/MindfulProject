using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MindfulApp.Models;
using MindfulApp.Services;

namespace MindfulApp.Services
{
    public class MoodService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:5265/api/MoodEntries";

        public MoodService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        public async Task<bool> CreateMoodAsync(MoodEntry mood)
        {
            try
            {
                var json = JsonConvert.SerializeObject(mood);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(baseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<MoodEntry>> GetAllMoodsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<MoodEntry>>(response) ?? new List<MoodEntry>();
            }
            catch { return new List<MoodEntry>(); }
        }

        public async Task<MoodEntry> GetMoodByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MoodEntry>(json);
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateMoodAsync(MoodEntry mood)
        {
            try
            {
                var json = JsonConvert.SerializeObject(mood);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{baseUrl}/{mood.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteMoodAsync(int id)
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
