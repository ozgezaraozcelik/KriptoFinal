using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KriptoFinal.Controllers
{
    public class Sha256Controller : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string dataType, string inputText, IFormFile inputFile)
        {
            string hashResult = "";

            try
            {
                byte[] data;

                if (dataType == "text")
                {
                    if (string.IsNullOrEmpty(inputText))
                    {
                        ViewBag.HashResult = "Metin alanı boş olamaz.";
                        return View();
                    }
                    data = Encoding.UTF8.GetBytes(inputText);
                }
                else if (dataType == "file")
                {
                    if (inputFile == null || inputFile.Length == 0)
                    {
                        ViewBag.HashResult = "Geçerli bir dosya seçilmedi.";
                        return View();
                    }

                    using (var ms = new MemoryStream())
                    {
                        await inputFile.CopyToAsync(ms);
                        data = ms.ToArray();
                    }
                }
                else
                {
                    ViewBag.HashResult = "Geçersiz veri türü.";
                    return View();
                }

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(data);
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        builder.Append(b.ToString("x2"));
                    }
                    hashResult = builder.ToString();
                }

                ViewBag.HashResult = hashResult;
            }
            catch (Exception ex)
            {
                ViewBag.HashResult = "Hata oluştu: " + ex.Message;
            }

            return View();
        }
    }
}
