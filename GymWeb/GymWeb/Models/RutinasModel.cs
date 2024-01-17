using GymWeb.Entities;
using Newtonsoft.Json;

namespace GymWeb.Models
{
    public class RutinasModel : IRutinasModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _urlApi;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;

        public RutinasModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _HttpContextAccessor = httpContextAccessor;
            _clientFactory = clientFactory;
        }

        public bool IngresarRutinas(RutinasEnt model)
        {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                string url = "api/Rutinas/AgregarRutinas";
                JsonContent jsonObject = JsonContent.Create(model);
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

        public List<RutinasEnt> ObtenerRutinas()
        {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                //Obtenemos base url almacenada en el appsettings
                var request = new HttpRequestMessage(HttpMethod.Get, _urlApi + $"api/Rutinas/ObtenerRutinas");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var client = _clientFactory.CreateClient("GYMPRO_Client");
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    StreamReader sr = new StreamReader(responseStream);
                    var result = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<List<RutinasEnt>>(result);
                    return data;
                }
                return new List<RutinasEnt>();
            }
            catch (Exception)
            {
                return new List<RutinasEnt>();
            }
        }

        public List<RutinasEnt> ObtenerRutinasTable()
        {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                //Obtenemos base url almacenada en el appsettings
                var request = new HttpRequestMessage(HttpMethod.Get, _urlApi + $"api/Rutinas/ObtenerRutinasTable");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var client = _clientFactory.CreateClient("GYMPRO_Client");
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    StreamReader sr = new StreamReader(responseStream);
                    var result = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<List<RutinasEnt>>(result);
                    return data;
                }
                return new List<RutinasEnt>();
            }
            catch (Exception)
            {
                return new List<RutinasEnt>();
            }
        }
    }
}
