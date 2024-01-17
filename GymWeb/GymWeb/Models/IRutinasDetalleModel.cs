using GymWeb.Entities;

namespace GymWeb.Models
{
    public interface IRutinasDetalleModel
    {
        public bool IngresarRutinasDetalle(RutinasDetalleEnt model);
        public List<RutinasDetalleEnt> ObtenerRutinasDetalle();
        public bool EliminarRegistro(int id);
    }
}
