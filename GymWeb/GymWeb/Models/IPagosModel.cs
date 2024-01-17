using GymWeb.Entities;

namespace GymWeb.Models
{
    public interface IPagosModel
    {
        public List<PagosEnt> getPagos(long idUsuario);
    }
}
