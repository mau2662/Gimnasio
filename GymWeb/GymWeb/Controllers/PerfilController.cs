using GymWeb.Config;
using GymWeb.Entities;
using GymWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GymWeb.Controllers
{
    [FilterConfig]
    public class PerfilController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUsuarioModel _usuarioModel;
        public PerfilController(ILogger<LoginController> logger, IUsuarioModel usuarioModel)
        {
            _logger = logger;
            _usuarioModel = usuarioModel;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userInfo") == null) {
                var data = new UsuarioEnt();
                return View(data);
            }
            var model = JsonConvert.DeserializeObject<UsuarioEnt>(HttpContext.Session.GetString("userInfo"));
            return View(model);
        }

        /// <summary>
        /// Me permite modificar la imagen de perfil de usuario
        /// </summary>
        /// <param name="Image"></param>
        /// <returns></returns>
        public ActionResult saveProfilePicture(IFormFile Image) {
            try
            {
                var model = JsonConvert.DeserializeObject<UsuarioEnt>(HttpContext.Session.GetString("userInfo"));
                byte[] fileByteArray;
                if (Image != null)
                {
                    using (var item = new MemoryStream())
                    {
                        Image.CopyTo(item);
                        fileByteArray = item.ToArray();
                    }
                    var response = _usuarioModel.AgregarFotoPerfil(fileByteArray, model.IdUsuario);
                    if (response > 0) {
                        model.FotoPerfil = fileByteArray;
                        HttpContext.Session.SetString("userInfo", JsonConvert.SerializeObject(model));
                        return Ok("Se ha procesado la solicitud correctamente");
                    }
                }
                return BadRequest("No se ha enviado información de la imagen");
            }
            catch (Exception)
            {
                return BadRequest("No se ha podido guardar su foto de perfil");
            }
        }

        /// <summary>
        /// Me pertite actualizar la informacion de nombre de usuario, correo y telefono
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult updateProfileInfo(UsuarioEnt model) {
            try
            {
                var response = _usuarioModel.ModificarInfoPerfil(model);
                if (response != null)
                {
                    TempData["success"] = "Se ha procesado la modificación de los datos de usuario con éxito";
                    var userData = JsonConvert.DeserializeObject<UsuarioEnt>(HttpContext.Session.GetString("userInfo"));

                    //Actualizo la informacion de la variable de sesion para que se actualice en la vista
                    userData.NombreCompleto = model.NombreCompleto;
                    userData.Correo = model.Correo;
                    userData.Telefono = model.Telefono;
                    HttpContext.Session.SetString("userInfo", JsonConvert.SerializeObject(userData));

                    return RedirectToAction("Index");
                }
                TempData["error"] = "No se logró modificar la información, favor intente más tarde";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["error"] = "Ha ocurrido un error al intentar modificar la información del usuario";
                return RedirectToAction("Index"); 
            }
        }




    }
}
