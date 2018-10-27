using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OfficeCulture.Data.Models;
using OfficeCulture.Sounds.Extensions;

namespace OfficeCulture.Sounds.Manager
{
    public class SlackFileManager
    {
        private readonly HttpClient _client = new HttpClient();
        
        public async Task<HttpResponseMessage> PostFileAsync(SlackFileUpload slackFileUpload)
        {
            string slackFileUploadJson = await Task.Run(() => JsonConvert.SerializeObject(slackFileUpload));
            var httpContentSlackFile = new StringContent(slackFileUploadJson, Encoding.UTF8, "application/json");
            return await _client.PostAsync("/api/files.upload", httpContentSlackFile);
        }

        public async Task RunAsync(SlackFileUpload slackFileUpload)
        {
            _client.BaseAddress = new Uri("https://slack.com");

            try
            {
                // Post the slack file
                 await PostFileAsync(slackFileUpload);
            }
            catch (Exception e)
            {
                // Error
            }
        }
    }
}