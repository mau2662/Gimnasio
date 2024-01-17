using GymWeb.Entities;

namespace GymWeb.Models
{
    public class EjercicioModel: IEjercicioModel
    {


        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _urlApi;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public EjercicioModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _HttpContextAccessor = httpContextAccessor;
        }


        public int CrearEjercicio(EjerciciosEnt entidad)
        {
            try
            {
                string url = _urlApi + "api/Ejercicio/CrearEjercicio";
                JsonContent obj = JsonContent.Create(entidad);
                var resp = _httpClient.PostAsync(url, obj).Result;

                if (resp.IsSuccessStatusCode)
                    return resp.Content.ReadFromJsonAsync<int>().Result;
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        //Fin del metodo


        public List<EjerciciosEnt>? ConsultarEjercicio()
        {
            string url = _urlApi + "api/Ejercicio/ConsultarEjercicio";
            var resp = _httpClient.GetAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<List<EjerciciosEnt>>().Result;
            else
                return null;
        }

        //Fin del metodo


        public int EliminarEjercicio(long q)
        {
            string url = _urlApi + "api/Ejercicio/EliminarEjercicio?q=" + q;

            var resp = _httpClient.DeleteAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;
        }
        //Fin del metodo

    }
}
