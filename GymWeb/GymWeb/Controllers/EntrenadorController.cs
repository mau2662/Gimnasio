using Microsoft.AspNetCore.Mvc;
using GymWeb.Models;
using GymWeb.Entities;
using Newtonsoft.Json;

namespace GymWeb.Controllers
{
    public class EntrenadorController : Controller
    {
        private readonly ILogger<EntrenadorController> _logger;
        private readonly IEntrenadorModel _entrenadorModel;
      
        public EntrenadorController(ILogger<EntrenadorController> logger, IEntrenadorModel entrenadorModel)
        {
            _logger = logger;
            _entrenadorModel = entrenadorModel;
           
        }

        [HttpGet]
        public IActionResult CrearCita()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult CrearCita(EntrenadorEnt entidad)
        {
            try
            {
                var resp = _entrenadorModel.CrearCita(entidad);
                if (resp == 1)
                    return RedirectToAction("EditarCitas", "Entrenador");
                //Respuesta si no se logro registrar el usuario
                ViewBag.MensajePantalla = "No se logro crear la cita";
                return View();
            }
            catch (Exception)
            {
                ViewBag.MensajePantalla = "Ha ocurrido un error al procesar la solicitud de creacion de citas";
                return View();
            }
        }

        //Fin del metodo

        [HttpGet]
        public IActionResult EditarCitas()
        {
            var datos = _entrenadorModel.EditarCitas();
            return View(datos);
        }

        //Fin del metodo

        [HttpGet]        
        public IActionResult EliminarCita(long q)
        {
            var entidad = new EntrenadorEnt();
            entidad.IdCita = q;

            _entrenadorModel.EliminarCita(entidad.IdCita);
            return RedirectToAction("EditarCitas", "Entrenador");
        }

        //Fin del metodo

  


    }
}
