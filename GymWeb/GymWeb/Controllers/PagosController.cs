using GymWeb.Entities;
using GymWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GymWeb.Controllers
{
    public class PagosController : Controller
    {

        private readonly ILogger<LoginController> _logger;
        private readonly IPagosModel _pagosModel;
        public PagosController(ILogger<LoginController> logger, IPagosModel pagosModel)
        {
            _logger = logger;
            _pagosModel = pagosModel;
        }


        public IActionResult Index()
        {
            var userInfo = JsonConvert.DeserializeObject<UsuarioEnt>(HttpContext.Session.GetString("userInfo"));
            var model = _pagosModel.getPagos(userInfo.IdUsuario);
            return View(model);
        }
    }
}
