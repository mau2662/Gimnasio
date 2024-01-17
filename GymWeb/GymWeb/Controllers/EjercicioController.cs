using GymWeb.Entities;
using GymWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymWeb.Controllers
{
    public class EjercicioController : Controller
    {

        private readonly ILogger<EjercicioController> _logger;
        private readonly IEjercicioModel _ejercicioModel;

        public EjercicioController(ILogger<EjercicioController> logger, IEjercicioModel entrenadorModel)
        {
            _logger = logger;
            _ejercicioModel = entrenadorModel;

        }


        [HttpGet]
        public IActionResult CrearEjercicio()
        {
            return View();
        }



        [HttpPost]
        public IActionResult CrearEjercicio(EjerciciosEnt entidad)
        {
            try
            {
                var resp = _ejercicioModel.CrearEjercicio(entidad);
                if (resp == 1)
                    return RedirectToAction("ConsultarEjercicio", "Ejercicio");

                ViewBag.MensajePantalla = "No se logro crear el ejercicio";
                return View();
            }
            catch (Exception)
            {
                ViewBag.MensajePantalla = "Ha ocurrido un error al procesar la solicitud de creacion de ejercicios";
                return View();
            }
        }

        //Fin del metodo


        [HttpGet]
        public IActionResult ConsultarEjercicio()
        {
            var datos = _ejercicioModel.ConsultarEjercicio();
            return View(datos);
        }

        //Fin del metodo

        [HttpGet]
        public IActionResult EliminarEjercicio(long q)
        {
            var entidad = new EjerciciosEnt();
            entidad.IdEjercicio = q;

            _ejercicioModel.EliminarEjercicio(entidad.IdEjercicio);
            return RedirectToAction("ConsultarEjercicio", "Ejercicio");
        }

        //Fin del metodo

    }
}
