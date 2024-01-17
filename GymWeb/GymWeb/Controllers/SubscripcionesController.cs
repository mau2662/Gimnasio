using GymWeb.Config;
using GymWeb.Entities;
using GymWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace GymWeb.Controllers
{
    [FilterConfig]
    public class SubscripcionesController : Controller
    {

        private readonly ISubscripcionModel _subscripcion;

        public SubscripcionesController(ISubscripcionModel subscripcion)
        {
            _subscripcion = subscripcion;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Realiza las acciones necesarias para confirmar el proceso de pago realizado
        /// </summary>
        /// <param name="idPlan"></param>
        /// <param name="monto"></param>
        /// <returns></returns>
        public ActionResult CreateSubscription(int idPlan, decimal monto) {
            try
            {
                var model = JsonConvert.DeserializeObject<UsuarioEnt>(HttpContext.Session.GetString("userInfo"));
                var request = new SubscripcionEnt
                {
                    IdUsuario = model.IdUsuario,
                    idPaquete = idPlan,
                    Descripcion = GetDescripcion(idPlan),
                    MontoPago = monto
                };
                var response = _subscripcion.addNewSubscription(request);
                //Actualizo la subscripcion del usuario
                var updateSub = _subscripcion.getSubscription(model.IdUsuario);
                HttpContext.Session.SetString("subscripcion", JsonConvert.SerializeObject(updateSub));
            }
            catch (Exception)
            {
            }
            return RedirectToAction("success");
        }

        public IActionResult success() {
            return View();
        }

        public IActionResult cancel()
        {
            return View();
        }

        /// <summary>
        /// Permite obtener la descripcion del pago realizar para crear la subscripcion, se separa para mayor modulacion
        /// </summary>
        /// <param name="idPlan"></param>
        /// <returns></returns>
        private string GetDescripcion(int idPlan) {
            try
            {
                string desc = string.Empty;
                switch (idPlan)
                {
                    case 1:
                        desc = "PLAN GOLD X MES 12M";
                        break;
                    case 2:
                        desc = "PLAN BLACK X MES 12M";
                        break;
                    case 3:
                        desc = "PLAN PLATINUM X MES 12M";
                        break;
                }
                return desc;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
