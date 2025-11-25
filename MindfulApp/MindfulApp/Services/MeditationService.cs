using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MindfulApp.Models;

namespace MindfulApp.Services
{
    public class MeditationService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl = "http://10.0.2.2:5265/api/MeditationSessions";

        public MeditationService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient(handler);
        }

        public async Task<bool> CreateSessionAsync(MeditationSession session)
        {
            try
            {
                var json = JsonConvert.SerializeObject(session);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(baseUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<MeditationSession>> GetAllSessionsAsync()
        {
            try
            {
                var response = await _client.GetStringAsync(baseUrl);
                return JsonConvert.DeserializeObject<List<MeditationSession>>(response) ?? new List<MeditationSession>();
            }
            catch { return new List<MeditationSession>(); }
        }

        public async Task<MeditationSession> GetSessionByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{baseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MeditationSession>(json);
                }
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> UpdateSessionAsync(MeditationSession session)
        {
            try
            {
                var json = JsonConvert.SerializeObject(session);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{baseUrl}/{session.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteSessionAsync(int id)
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
