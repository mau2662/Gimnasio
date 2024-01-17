using Microsoft.AspNetCore.Mvc;
using GymWeb.Models;
using GymWeb.Entities;
using Newtonsoft.Json;

namespace GymWeb.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILogger<LoginController> _logger;
        private readonly IUsuarioModel _usuarioModel;
        private readonly ISubscripcionModel _subscripcionModel;

        public LoginController(ILogger<LoginController> logger, IUsuarioModel usuarioModel, ISubscripcionModel subscripcionModel)
        {
            _logger = logger;
            _usuarioModel = usuarioModel;
            _subscripcionModel = subscripcionModel;
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult InicioSesion(UsuarioEnt entidad)
        {
            try
            {
                var datos = _usuarioModel.InicioSesion(entidad);

                if (datos?.Codigo != 1)
                {
                    ViewBag.Mensaje = datos?.Mensaje;
                    return View("Login");
                }

                HttpContext.Session.SetString("RolUsuario", datos.Objeto.NombreRol??"");
                HttpContext.Session.SetString("Token", datos.Objeto.Token??"");
                HttpContext.Session.SetString("userInfo",JsonConvert.SerializeObject(datos.Objeto));

                var subs = _subscripcionModel.getSubscription(datos.Objeto.IdUsuario);


                HttpContext.Session.SetString("subscripcion",JsonConvert.SerializeObject(subs));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Consulte con el administrador";
                return View("Login");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegistrarUsuario() {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuario(UsuarioEnt entidad)
        {
            try
            {
                var resp = _usuarioModel.RegistrarUsuario(entidad);
                if (resp == 1)
                {
                    return Json(new { success = true, redirectUrl = Url.Action("Login", "Login") });
                }
                else
                {
                    return Json(new { success = false, errorMessage = "No se logró registrar el usuario" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, errorMessage = "Ha ocurrido un error al procesar la solicitud de registro de usuario" });
            }
        }

        [HttpGet]
        public IActionResult RecuperarContrasenna()
        {
            return View();
        }
           
        [HttpPost]
        public IActionResult RecuperarContrasenna(UsuarioEnt entidad)
        {
            var resp = _usuarioModel.RecuperarContrasenna(entidad);
            if (resp == 1)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ViewBag.MensajePantalla = "No se logro recuperar el usuario";
                return View();
            }
        }

        [HttpGet]
        public IActionResult CambiarClaveCuenta(string q)
        {
            UsuarioEnt entidad = new UsuarioEnt();
            entidad.IdUsuario = (int)long.Parse(q);
            return View(entidad);
        }

        [HttpPost]
        public IActionResult CambiarClaveCuenta(UsuarioEnt entidad)
        {
            var resp = _usuarioModel.CambiarClaveCuenta(entidad);
            if (resp == 1)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                ViewBag.MensajePantalla = "No se logro cambiar su contraseña";
                return View();
            }
        }

    }
}
