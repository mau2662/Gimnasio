using GymWeb.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymWeb.Models
{
    public interface IEntrenadorModel
    {

        public int CrearCita(EntrenadorEnt entidad);

        public List<EntrenadorEnt>? EditarCitas();

        public int EliminarCita(long q);


    }
}
