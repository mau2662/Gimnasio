using GymWeb.Entities;
using GymWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace GymWeb.Controllers
{
    public class UsuarioController : Controller
    {


        private readonly IUsuarioModel _usuarioModel;
        public UsuarioController(IUsuarioModel usuarioModel)
        {
            _usuarioModel = usuarioModel;
        }


        [HttpGet]
        public IActionResult Index()
        {

            var datos = _usuarioModel.ConsultarUsuarios();
            return View(datos?.Objetos);


        }


        [HttpGet]
        //[FiltroSeguridad]
        public IActionResult ActualizarEstadoUsuario(int IdUsuario, bool Estado)
        {
            var entidad = new UsuarioEnt();
            entidad.IdUsuario = IdUsuario;
            entidad.Estado = Estado;


            var resultadoActualizacion = _usuarioModel.ActualizarEstadoUsuario(entidad.IdUsuario, entidad.Estado).Result;



            return RedirectToAction("Index", "Usuario");
        }

        [HttpGet]
        //[FiltroSeguridad]
        public IActionResult ModificarUsuario(long q)
        {
            var datos = _usuarioModel.ConsultarUsuario(q);
            var usuarioRespuesta = _usuarioModel.ConsultarRoles();

            // Verifica que la respuesta no sea nula y que la lista de roles dentro de Objetos no sea nula antes de pasarla a la vista
            ViewData["Roles"] = usuarioRespuesta?.Objetos?.Select(u => new UsuarioEnt { IdRol = u.IdRol, NombreRol = u.NombreRol }).ToList() ?? new List<UsuarioEnt>();

            return View(datos?.Objeto);
        }



        [HttpPost]
        public ActionResult ModificarUsuario(UsuarioEnt entidad)
        {
            try
            {
                var response = _usuarioModel.ModificarUsuario(entidad);

                if (response != null)
                {
                    TempData["success"] = "Se ha procesado la modificación de los datos de usuario con éxito";

                    var datos = _usuarioModel.ConsultarUsuario(entidad.IdUsuario);

                    //ViewData["CategoriaGasto"] = datos?.Objeto?.GA_CATEGORIA_GASTO;

                    return RedirectToAction("Index", "Usuario");


                }
                TempData["error"] = "No se logró modificar la información, favor intente más tarde";
                return RedirectToAction("Index", "Usuario");
            }
            catch (Exception)
            {
                TempData["error"] = "Ha ocurrido un error al intentar modificar la información del usuario";
                return RedirectToAction("Index");
            }

        }



        [HttpGet]
        //[FiltroSeguridad]
        public IActionResult CambiarEstadoUsuario(int IdUsuario, bool Estado)
        {
            var entidad = new UsuarioEnt();
            entidad.IdUsuario = IdUsuario;
            entidad.Estado = Estado;


            var resultadoActualizacion = _usuarioModel.ActualizarEstadoUsuario(entidad.IdUsuario, entidad.Estado).Result;

            TempData["success"] = "Se ha realizado el cambio de estado";

            var datos = _usuarioModel.ConsultarUsuario(IdUsuario);

            return RedirectToAction("Index", "Usuario" );
        }




        [HttpGet]
        //[FiltroSeguridad]
        public IActionResult EliminarUsuario(int IdUsuario)
        {
            var entidad = new UsuarioEnt();
            entidad.IdUsuario = IdUsuario;


            var resultadoActualizacion = _usuarioModel.EliminarUsuario(entidad.IdUsuario).Result;

            TempData["success"] = "Se ha eliminado el usuario con exito";

            return RedirectToAction("Index", "Usuario");
        }

    }
}