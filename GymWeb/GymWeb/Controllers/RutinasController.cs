using GymWeb.Entities;
using GymWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymWeb.Controllers
{
    public class RutinasController : Controller
    {
        private readonly IRutinasModel _sut;
        private readonly IRutinasDetalleModel _sutDetalle;
        private readonly IEjercicioModel _ejercicio;

        public RutinasController(IRutinasModel sut, IRutinasDetalleModel sutDetalle, IEjercicioModel ejercicio = null)
        {
            _sut = sut;
            _sutDetalle = sutDetalle;
            _ejercicio = ejercicio;
        }

        public IActionResult Index()
        {
            var model = new RutinasView();
            model.Rutinas = new List<RutinasEnt>();
            model.DetalleRutinas = new List<RutinasDetalleEnt>();
            model.Ejercicios = new List<EjerciciosEnt>();
            model.RutinasTable = new List<RutinasEnt>();

            model.Rutinas = _sut.ObtenerRutinas();
            model.DetalleRutinas = _sutDetalle.ObtenerRutinasDetalle();
            model.Ejercicios = _ejercicio.ConsultarEjercicio();
            model.RutinasTable = _sut.ObtenerRutinasTable();
            model.RutinaSeleccion = new RutinasEnt();
            model.RutinaDSeleccion = new RutinasDetalleEnt();

            return View(model);
        }

        public ActionResult EliminarEjercicio(int id) {
            try
            {
                var response = _sutDetalle.EliminarRegistro(id);
                if (response)
                {
                    return RedirectToAction("Index");
                }
                else {
                    ViewBag.Error = "No se ha logrado ingresar el registro solicitado";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Ha ocurrido un error al procesar la solicitud";
                return RedirectToAction("Index");
            }
        }

        public ActionResult AgregarRutina(RutinasView model) {
            try
            {
                var response = _sut.IngresarRutinas(model.RutinaSeleccion);
                if (response)
                {
                    return RedirectToAction("Index");
                }
                else {
                    ViewBag.Error = "No se ha logrado ingresar el registro solicitado";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Ha ocurrido un error al procesar la solicitud";
                return RedirectToAction("Index");
            }
        }

        public ActionResult AgregarRutinaDetalle(RutinasView model)
        {
            try
            {
                var response = _sutDetalle.IngresarRutinasDetalle(model.RutinaDSeleccion);
                if (response)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "No se ha logrado ingresar el registro solicitado";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Ha ocurrido un error al procesar la solicitud";
                return RedirectToAction("Index");
            }
        }
    }
}
