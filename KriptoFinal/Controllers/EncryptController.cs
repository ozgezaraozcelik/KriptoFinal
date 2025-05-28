using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KriptoFinal.Controllers
{
    public class EncryptController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string plainText, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
            {
                ViewBag.EncryptedText = "Genel anahtar gerekli.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(plainText))
            {
                ViewBag.EncryptedText = "Şifrelenecek metin boş olamaz.";
                return View();
            }

            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
                byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

                    int maxLength = rsa.KeySize / 8 - 11; // PKCS#1 padding ile sınırlı
                    if (dataToEncrypt.Length > maxLength)
                    {
                        ViewBag.EncryptedText = $"Metin çok büyük. Maksimum şifrelenebilir boyut: {maxLength} byte.";
                        return View();
                    }

                    byte[] encryptedBytes = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
                    ViewBag.EncryptedText = Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (FormatException)
            {
                ViewBag.EncryptedText = "Genel anahtar formatı hatalı (Base64 formatında olmalı).";
            }
            catch (CryptographicException ce)
            {
                ViewBag.EncryptedText = "Şifreleme hatası: " + ce.Message;
            }
            catch (Exception ex)
            {
                ViewBag.EncryptedText = "Beklenmeyen hata: " + ex.Message;
            }

            return View();
        }
    }
}
