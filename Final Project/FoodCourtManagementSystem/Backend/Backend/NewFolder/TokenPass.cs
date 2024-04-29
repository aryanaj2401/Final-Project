using System.Net.Http.Headers;

namespace BackendUsers.NewFolder
{
    public static class TokenPass
    {

        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient(string token)
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Split(" ")[1]);
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


    }
}
