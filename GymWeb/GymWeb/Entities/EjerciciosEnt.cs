namespace GymWeb.Entities
{
    public class EjerciciosEnt
    {
        public long IdEjercicio { get; set; }
        public string NombreEjercicio { get; set; } = string.Empty;
        public string DescripcionEjercicio { get; set; } = string.Empty;
        public string VideoEjercicio { get; set; } = string.Empty;
    }
}
