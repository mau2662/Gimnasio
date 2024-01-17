using GymWeb.Entities;
using Newtonsoft.Json;

namespace GymWeb.Models
{
    public class RutinasDetalleModel : IRutinasDetalleModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _urlApi;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;

        public RutinasDetalleModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _HttpContextAccessor = httpContextAccessor;
            _clientFactory = clientFactory;
        }


        public bool IngresarRutinasDetalle(RutinasDetalleEnt model)
        {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                string url = "api/RutinasDetalle/AgregarRutinas";
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

        public bool EliminarRegistro(int id) {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                //Obtenemos base url almacenada en el appsettings
                var request = new HttpRequestMessage(HttpMethod.Delete, _urlApi + $"api/RutinasDetalle/EliminarRutina?id={id}");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var client = _clientFactory.CreateClient("GYMPRO_Client");
                var response = client.SendAsync(request).Result;

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

        public List<RutinasDetalleEnt> ObtenerRutinasDetalle()
        {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                //Obtenemos base url almacenada en el appsettings
                var request = new HttpRequestMessage(HttpMethod.Get, _urlApi + $"api/RutinasDetalle/ObtenerRutinas");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var client = _clientFactory.CreateClient("GYMPRO_Client");
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    StreamReader sr = new StreamReader(responseStream);
                    var result = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<List<RutinasDetalleEnt>>(result);
                    return data;
                }
                return new List<RutinasDetalleEnt>();
            }
            catch (Exception)
            {
                return new List<RutinasDetalleEnt>();
            }
        }
    }
}
