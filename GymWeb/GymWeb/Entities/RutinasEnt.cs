namespace GymWeb.Entities
{
    public class RutinasEnt
    {
        public int IdRutina { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacionRuta { get; set; }
        public bool Estado { get; set; }
        public string Intensidad { get; set; }

        //Para tablas de rutinas
        public int? id { get; set; }
        public string? NombreRutina { get; set; }
        public string? NombreEjercicio { get; set; }
        public DateTime? FechaCreacionRutina { get; set; }
    }
}
