
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Model
{
    public class RequestModel
    {
        private static string Link = "http://integration.cdek.ru/v1/location/cities/json?fiasGuid=";
        private static string CalculateLink = "http://api.cdek.ru/calculator/calculate_tarifflist.php";
        private static HttpClient httpClient = new HttpClient();
        public string Version { get; private set; } = "1.0";
        public int SenderCityId { get; private set; }
        public int ReceiverCityId { get; private set; }
        public int TariffId { get; private set; } = 1;
        public List<Goods> Goods { get; private set; } = new List<Goods>();
        public RequestModel(Guid senderGuid, Guid recieverGuid, float weight, int length, int width, int height)
        {
            GetCitiesId(senderGuid, recieverGuid).Wait();
            Goods.Add(new Goods(weight/1000, length/10, width/10, height / 10));
        }
        private async Task GetCitiesId(Guid senderGuid, Guid recieverGuid)
        {
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, Link + senderGuid);
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                dynamic json = JArray.Parse(content);
                try
                {
                    SenderCityId = json[0].cityCode;
                }
                catch (Exception){}
            }
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, Link + recieverGuid);
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                dynamic json = JArray.Parse(content);
                try
                {
                    ReceiverCityId = json[0].cityCode;
                }
                catch (Exception){}
            }
        }
        public async Task<string> CalculatePrice()
        {
            var payload = JsonSerializer.Serialize(this);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(CalculateLink, content);
            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic json = JObject.Parse(responseContent);
            return json.result;
        }
    }
}
