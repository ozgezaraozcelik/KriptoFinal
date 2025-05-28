using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KriptoFinal.Controllers
{
    public class DecryptController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string encryptedText, string privateKey)
        {
            if (string.IsNullOrEmpty(encryptedText) || string.IsNullOrEmpty(privateKey))
            {
                ViewBag.DecryptedText = "Şifreli metin ve özel anahtar gerekli.";
                return View();
            }

            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                    byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                    string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                    ViewBag.DecryptedText = decryptedText;
                }
            }
            catch (FormatException)
            {
                ViewBag.DecryptedText = "Girdi formatı hatalı (Base64 olmalı).";
            }
            catch (CryptographicException ce)
            {
                ViewBag.DecryptedText = "Şifre çözme hatası: " + ce.Message;
            }
            catch (Exception ex)
            {
                ViewBag.DecryptedText = "Beklenmeyen hata: " + ex.Message;
            }

            return View();
        }
    }
}
