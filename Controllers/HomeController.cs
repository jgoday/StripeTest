using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using StripeTest.Models;

namespace StripeTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Success()
        {
            Console.WriteLine("Success !!");

            return View();
        }

        [HttpGet]
        public IActionResult Charge()
        {
            try
            {
                var options = new SessionCreateOptions {
                    PaymentMethodTypes = new List<string> {
                        "card",
                    },
                    LineItems = new List<SessionLineItemOptions> {
                        new SessionLineItemOptions {
                            Name = "T-shirt",
                            Description = "Comfortable cotton t-shirt",
                            Amount = 500,
                            Currency = "eur",
                            Quantity = 1,
                        },
                    },
                    SuccessUrl = "https://localhost:5001/home/success",
                    CancelUrl = "https://localhost:5001/home/cancel",
                };

                var service = new SessionService();
                Session session = service.Create(options);

                return Ok(session);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                return Ok(new
                {
                    Error=e.Message
                });
            }
        }
    }
}
