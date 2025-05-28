using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KriptoFinal.Controllers
{
    public class GenerateKeyController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(object dummy = null)
        {
            using (RSA rsa = RSA.Create(2048))
            {
                // Public Key
                string publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());

                // Private Key
                string privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());

                ViewBag.PublicKey = publicKey;
                ViewBag.PrivateKey = privateKey;
            }

            return View();
        }
    }
}
