using GymWeb.Entities;

namespace GymWeb.Models
{
    public interface ISubscripcionModel
    {
        /// <summary>
        /// Registra las subscripciones generadas al realizar el pago por medio del sistema
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool addNewSubscription(SubscripcionEnt request);

        /// <summary>
        /// Permite obtener la informacion de la subscripcion actual del cliente
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public SubscripcionEnt getSubscription(long idUsuario);
    }
}
