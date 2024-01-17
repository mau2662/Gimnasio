namespace GymWeb.Entities
{
    public class RutinasView
    {
        public RutinasEnt RutinaSeleccion { get; set; }
        public RutinasDetalleEnt RutinaDSeleccion { get; set; }
        public List<RutinasDetalleEnt> DetalleRutinas { get; set; }
        public List<RutinasEnt> Rutinas { get; set; }
        public List<RutinasEnt> RutinasTable { get; set; }
        public List<EjerciciosEnt> Ejercicios { get; set; }
    }
}
