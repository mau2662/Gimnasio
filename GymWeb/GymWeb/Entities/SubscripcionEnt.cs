namespace GymWeb.Entities
{
    public class SubscripcionEnt
    {
        public long IdUsuario { get; set; }
        public int idPaquete { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCaducidadInscripcion { get; set; }
        public string? Nombre { get; set; }
        public decimal MontoPago { get; set; }
    }
}
