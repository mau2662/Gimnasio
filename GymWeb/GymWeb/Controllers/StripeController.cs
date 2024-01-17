using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace GymWeb.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class StripeController : Controller
    {
        [HttpPost]
        [Route("CreatePlanGold")]
        public ActionResult CreatePlanGold()
        {
            var domain = "https://localhost:7214";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price = "price_1O6kneERe0Ut4IOaJp2uDA77",
                    Quantity = 1,
                  },
                },
                Mode = "subscription",
                Locale = "es",
                SuccessUrl = domain + "/Subscripciones/CreateSubscription?idPlan=1&monto=20000",
                CancelUrl = domain + "/Subscripciones/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }


        [HttpPost]
        [Route("CreatePlanBlack")]
        public ActionResult CreatePlanBlack()
        {
            var domain = "https://localhost:7214";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price = "price_1O6kmLERe0Ut4IOaRv38TiMQ",
                    Quantity = 1,
                  },
                },
                Mode = "subscription",
                Locale = "es",
                SuccessUrl = domain + "/Subscripciones/CreateSubscription?idPlan=2&monto=30000",
                CancelUrl = domain + "/Subscripciones/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpPost]
        [Route("CreatePlanPlatinum")]
        public ActionResult CreatePlanPlatinum()
        {
            var domain = "https://localhost:7214";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price = "price_1O6koJERe0Ut4IOaL6n4JFMM",
                    Quantity = 1,
                  },
                },
                Mode = "subscription",
                Locale = "es",
                SuccessUrl = domain + "/Subscripciones/CreateSubscription?idPlan=3&monto=45000",
                CancelUrl = domain + "/Subscripciones/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}
