using GymWeb.Entities;
using Newtonsoft.Json;

namespace GymWeb.Models
{
    public class PagosModel : IPagosModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _urlApi;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;

        public PagosModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _HttpContextAccessor = httpContextAccessor;
            _clientFactory = clientFactory;
        }

        public List<PagosEnt> getPagos(long idUsuario)
        {
            try
            {
                string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                //Obtenemos base url almacenada en el appsettings
                var request = new HttpRequestMessage(HttpMethod.Get, _urlApi + $"api/Pagos/ObtenerPagos?idUsuario={idUsuario}");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var client = _clientFactory.CreateClient("GYMPRO_Client");
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = response.Content.ReadAsStreamAsync().Result;
                    StreamReader sr = new StreamReader(responseStream);
                    var result = sr.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<List<PagosEnt>>(result);
                    return data;
                }
                return new List<PagosEnt>();
            }
            catch (Exception)
            {
                return new List<PagosEnt>();
            }
        }
    }
}
