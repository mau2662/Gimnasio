using GymWeb.Entities;

namespace GymWeb.Models
{
    public interface IRutinasModel
    {
        public bool IngresarRutinas(RutinasEnt model);
        public List<RutinasEnt> ObtenerRutinas();
        public List<RutinasEnt> ObtenerRutinasTable();
    }
}
