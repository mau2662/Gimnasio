using GymWeb.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

namespace GymWeb.Models
{
    public class EntrenadorModel : IEntrenadorModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _urlApi;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public EntrenadorModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _HttpContextAccessor = httpContextAccessor;
        }

        public int CrearCita(EntrenadorEnt entidad)
        {
            try
            {
                string url = _urlApi + "api/Entrenador/CrearCita";
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



        public List<EntrenadorEnt>? EditarCitas()
        {
            string url = _urlApi + "api/Entrenador/EditarCitas";
            var resp = _httpClient.GetAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<List<EntrenadorEnt>>().Result;
            else
                return null;
        }

        //Fin del metodo

        public int EliminarCita(long q)
        {
            string url = _urlApi + "api/Entrenador/EliminarCita?q=" + q;
  
            var resp = _httpClient.DeleteAsync(url).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<int>().Result;
            else
                return 0;
        }
        //Fin del metodo





    }
}
