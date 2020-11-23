using Encriptacao.Context;
using Encriptacao.Migrations;
using Encriptacao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UsuarioModel = Encriptacao.Models.UsuarioModel;

namespace Encriptacao.Controllers
{
    public class LoginController : Controller
    {

        private Contexto db = new Contexto();
        private static string AesIV256BD = @"%j?TmFP6$BbMnY$@";//16 caracteres
        private static string AesKey256BD = @"rxmBUJy]&,;3jKwDTzf(cui$<nc2EQr)";//32 caracteres
        // GET: Login
        public ActionResult Index(string? erro)
        {
            if (erro != null)
            {
                TempData["error"] = erro;
            }
            return View();
        }

        #region - INDEX
        [HttpPost]
        public ActionResult Verificar(Models.UsuarioModel usuarioModel)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] src = Encoding.Unicode.GetBytes(usuarioModel.Email);

            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                //Converte byte array para string de base 64
                usuarioModel.Email = Convert.ToBase64String(dest);
            }

            Models.UsuarioModel Consulta = db.Usuarios.FirstOrDefault
                (u => u.Email == usuarioModel.Email);

            string erro = "Usuario ou Senha Inválido";

            if (Consulta == null)
            {
                return RedirectToAction(nameof(Index), new { @erro = erro });
            }

            if (BCrypt.Net.BCrypt.Verify(usuarioModel.Senha, Consulta.Senha))
            {
                Session["Nome"] = Consulta.Nome;
                Session["Nivel"] = Consulta.Nivel;
                return RedirectToAction("Index", "Usuario");
            }

            return RedirectToAction(nameof(Index), new { @erro = erro });
        }
        #endregion
    }
}