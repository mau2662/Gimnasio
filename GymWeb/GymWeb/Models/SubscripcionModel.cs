using GymWeb.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GymWeb.Models
{
    public class SubscripcionModel : ISubscripcionModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _urlApi;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;

        public SubscripcionModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _HttpContextAccessor = httpContextAccessor;
            _clientFactory = clientFactory;
        }

        public bool addNewSubscription(SubscripcionEnt request)
        {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                var model = new SubscripcionEnt();
                string url = "api/Subscripciones/AgregarSubscripcion";
                JsonContent jsonObject = JsonContent.Create(request);
                var requestURL = new HttpRequestMessage(HttpMethod.Post, _urlApi + url);

                requestURL.Headers.Add("Authorization", $"Bearer {token}");
                requestURL.Content = jsonObject;

                var client = _clientFactory.CreateClient("GYMPRO_Client");
                var response = client.SendAsync(requestURL).Result;

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    StreamReader sr = new StreamReader(responseStream);
                    var result = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<bool>(result);
                    return data;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SubscripcionEnt getSubscription(long idUsuario) {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                //Obtenemos base url almacenada en el appsettings
                var request = new HttpRequestMessage(HttpMethod.Get, _urlApi + $"api/Subscripciones/ObtenerSubscripcion?idUsuario={idUsuario}");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var client = _clientFactory.CreateClient("GYMPRO_Client");
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    StreamReader sr = new StreamReader(responseStream);
                    var result = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<SubscripcionEnt>(result);
                    return data;
                }
                return new SubscripcionEnt();
            }
            catch (Exception)
            {
                return new SubscripcionEnt();
            }
        }
    }
}
