using GymWeb.Entities;

namespace GymWeb.Models
{
    public interface IEjercicioModel
    {
        public int CrearEjercicio(EjerciciosEnt entidad);

        public List<EjerciciosEnt>? ConsultarEjercicio();

        public int EliminarEjercicio(long q);


    }
}
