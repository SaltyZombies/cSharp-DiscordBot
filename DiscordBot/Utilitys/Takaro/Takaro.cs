using RestSharp;
using DiscordBot;
using DiscordBot.Services;

namespace DiscordBot.Utilitys.Takaro
{
    internal class Takaro
    {
        private readonly RestClient _takaroClient;
        private readonly string _apiKey;
        private static ILogger<DiscordBotService> _logger = DiscordBotService.GetLogger();

        public Takaro(string apiKey)
        {
            _takaroClient = new RestClient("https://api.takaro.io");
            _apiKey = apiKey;
        }

        private RestRequest CreateAuthenticatedRequest(string resource, Method method)
        {
            try
            {
                var request = new RestRequest(resource, method);
                request.AddHeader("Authorization", $"Bearer {_apiKey}");
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating authenticated request");
                throw;
            }
        }

        public void CallApi1()
        {
            try
            {
                var request = CreateAuthenticatedRequest("/api1", Method.Get);
                var response = _takaroClient.Execute(request);
                // Handle the response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling API1");
                throw;
            }
        }

        public void CallApi2(string param1, int param2)
        {
            try
            {
                var request = CreateAuthenticatedRequest("/api2", Method.Post);
                request.AddParameter("param1", param1);
                request.AddParameter("param2", param2);
                var response = _takaroClient.Execute(request);
                // Handle the response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling API2");
                throw;
            }
        }

        public void CallApi3()
        {
            try
            {
                var request = CreateAuthenticatedRequest("/api3", Method.Get);
                var response = _takaroClient.Execute(request);
                // Handle the response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling API3");
                throw;
            }
        }
    }
}
