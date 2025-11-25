using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MindfulApp.Models;

namespace MindfulApp.Services
{
    public class AffirmationService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:5265/api/AffirmationEntries";

        public AffirmationService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        public async Task<bool> CreateAffirmationAsync(AffirmationEntry affirmation)
        {
            try
            {
                var json = JsonConvert.SerializeObject(affirmation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(baseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<AffirmationEntry>> GetAllAffirmationsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<AffirmationEntry>>(response) ?? new List<AffirmationEntry>();
            }
            catch { return new List<AffirmationEntry>(); }
        }

        public async Task<AffirmationEntry> GetAffirmationByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AffirmationEntry>(json);
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateAffirmationAsync(AffirmationEntry affirmation)
        {
            try
            {
                var json = JsonConvert.SerializeObject(affirmation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{baseUrl}/{affirmation.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteAffirmationAsync(int id)
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
