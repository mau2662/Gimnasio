namespace GymWeb.Entities
{
    public class PagosEnt
    {
        public long IdPago { get; set; }
        public long IdUsuario { get; set; }
        public int IdPaquete { get; set; }
        public string? Descripcion { get; set; }
        public double MontoPago { get; set; }
        public DateTime FechaCreacionPago { get; set; }
        public string? Nombre { get; set; }
    }
}
