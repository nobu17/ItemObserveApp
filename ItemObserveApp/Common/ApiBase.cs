using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ItemObserveApp.Common
{
    public class APIResult<T> where T : class
    {
        public T Result { get; set; }

        public string Json { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class APIClinet<T> where T : class
    {
        private const string AuthHeaderKey = "Authorization";

        public string URL { get; set; }

        public APIClinet()
        {
        }

        public string ApiToken { get; set; }

        public async Task<APIResult<T>> GetAsync(Dictionary<string, string> urlParam)
        {
            var httpClient = new HttpClient();
            var paramStrs = await new FormUrlEncodedContent(urlParam).ReadAsStringAsync();

            var request = new HttpRequestMessage(HttpMethod.Get, URL + paramStrs);

            if (!string.IsNullOrWhiteSpace(ApiToken))
                request.Headers.Add(AuthHeaderKey, ApiToken);

            var response = await httpClient.SendAsync(request);
            var apiRes = new APIResult<T>();
            apiRes.StatusCode = response.StatusCode;
            if (response.IsSuccessStatusCode)
            {
                apiRes.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            return apiRes;
        }

        public async Task<APIResult<T>> PostAsync(Object reqObj)
        {
            var httpClient = new HttpClient();
            var jsonStr = JsonConvert.SerializeObject(reqObj);

            var request = new HttpRequestMessage(HttpMethod.Post, URL);
            request.Headers.Add("ContentType", "application/json");

            if (!string.IsNullOrWhiteSpace(ApiToken))
                request.Headers.Add(AuthHeaderKey, ApiToken);

            request.Content = new StringContent(jsonStr, Encoding.UTF8, @"application/json");

            var response = await httpClient.SendAsync(request);
            var apiRes = new APIResult<T>();
            apiRes.StatusCode = response.StatusCode;
            apiRes.Json = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnAuthException();
            }
            if (response.IsSuccessStatusCode)
            {
                apiRes.Result = JsonConvert.DeserializeObject<T>(apiRes.Json);
            }
            return apiRes;
        }
    }

    public class UnAuthException : Exception
    {
        public UnAuthException()
        {
        }
    }
}
