namespace GymWeb.Entities
{
    public class RutinasDetalleEnt
    {
        public int id { get; set; }
	    public int idRutina { get; set; }
        public int idEjercicio { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
