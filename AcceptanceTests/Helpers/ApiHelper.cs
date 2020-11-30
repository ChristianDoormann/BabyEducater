using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AcceptanceTests.Contracts;
using AcceptanceTests.Infrastructure;

namespace AcceptanceTests.Helpers
{
    public class ApiHelper
    {
        private Uri _baseUrl;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiHelper(
            TestEnvironment testEnvironment,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _baseUrl = new Uri(testEnvironment.RestApiUrl);
        }
        
        public async Task<(HttpStatusCode statusCode, CreateBabyResponse babyResponse)> CreateBaby(string name, int age)
        {
            var request = PrepareRequest(HttpMethod.Post, "v1/baby", new CreateBabyRequest
            {
                Name = name,
                Age = age
            });
            return await SendRequestAndParseResponse<CreateBabyResponse>(request);
        }
        
        public async Task<(HttpStatusCode statusCode, GetBabyResponse response)> QueryBaby(Guid babyId)
        {
            var request = PrepareRequest(HttpMethod.Get, $"v1/baby/{babyId}");
            return await SendRequestAndParseResponse<GetBabyResponse>(request);
        }

        public async Task<HttpStatusCode> DeleteBaby(Guid babyId)
        {
            var request = PrepareRequest(HttpMethod.Delete, $"v1/baby/{babyId}");
            return (await SendRequest(request)).StatusCode;
        }

        public async Task<HttpStatusCode> AddFamilyMembers(Guid babyId, IEnumerable<string> familyMembers)
        {
            var request = PrepareRequest(HttpMethod.Post, $"v1/baby/{babyId}/familymembers", new AddFamilyMembersRequest
            {
                Names = familyMembers
            });
            return (await SendRequest(request)).StatusCode;
        }

        public async Task<(HttpStatusCode statusCode, AddWordsToVocabularyResponse response)> AddWordsToVocabulary(Guid babyId, string transcription)
        {
            var request = PrepareRequest(HttpMethod.Post, $"v1/baby/{babyId}/vocabulary", new AddWordsToVocabularyRequest
            {
                Transcription = transcription
            });
            return await SendRequestAndParseResponse<AddWordsToVocabularyResponse>(request);
        }

        private HttpRequestMessage PrepareRequest(
            HttpMethod method,
            string url,
            object content = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (content == null) 
                return request;
            
            var jsonBody = JsonSerializer.Serialize(content);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, 
                new MediaTypeWithQualityHeaderValue("application/json").MediaType);

            return request;
        }

        private async Task<(HttpStatusCode statusCode, TResponse response)> SendRequestAndParseResponse<TResponse>(
            HttpRequestMessage request) where TResponse : class
        {
            var response = await SendRequest(request);
            if (!response.IsSuccessStatusCode)
                return (response.StatusCode, null as TResponse);
            
            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            return (response.StatusCode, result);
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = _baseUrl;

            return await httpClient.SendAsync(request);
        }
    }
}