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
    public class UsuarioController : Controller
    {

        private Contexto db = new Contexto();
        private static string AesIV256BD = @"%j?TmFP6$BbMnY$@";//16 caracteres
        private static string AesKey256BD = @"rxmBUJy]&,;3jKwDTzf(cui$<nc2EQr)";//32 caracteres

        #region Region Index
        // GET: Usuario
        public ActionResult Index()
        {
            List<Models.UsuarioModel> usuarios = db.Usuarios.ToList();

            //AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            for (int i = 0; i < usuarios.Count; i++)
            {
                byte[] src = Convert.FromBase64String(usuarios[i].Email);
                using (ICryptoTransform decrypt = aes.CreateDecryptor())
                {
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    usuarios[i].Email = Encoding.Unicode.GetString(dest);
                }
            }

            return View(usuarios.ToList());
        }
        #endregion

        #region Region -Create - GET
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Region - Create - Post

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Models.UsuarioModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                //Hash da Senha
                usuarioModel.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioModel.Senha);
                usuarioModel.ConfirmarSenha = usuarioModel.Senha;

                //AesCryptoServiceProvider
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
                aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                //Convertendo string para byte array
                byte[] src = Encoding.Unicode.GetBytes(usuarioModel.Email);


                //Encriptação
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                    //Converte byte array para string de base 64
                    usuarioModel.Email = Convert.ToBase64String(dest);
                }
                db.Usuarios.Add(usuarioModel);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioModel);
        }
        #endregion

        #region Details - GET

        [HttpGet]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encriptacao.Models.UsuarioModel usuarioModel = db.Usuarios.Find(id);
            if (usuarioModel == null)
            {
                return HttpNotFound();
            }

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Converter a String para um byte array 64 bists
            byte[] src = Convert.FromBase64String(usuarioModel.Email);

            // Decriptar
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                usuarioModel.Email = Encoding.Unicode.GetString(dest);
            }
            return View(usuarioModel);
        }
        #endregion

        #region Post -EDIT - GET

        [HttpGet]

        public ActionResult edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Encriptacao.Models.UsuarioModel usuarioModel = db.Usuarios.Find(id);

            if (usuarioModel == null)
            {
                return HttpNotFound();
            }

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            //Convertendo string para byte array
            byte[] src = System.Convert.FromBase64String(usuarioModel.Email);


            // DEncriptação
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                //Converte byte array para string de base 64
                usuarioModel.Email = Encoding.Unicode.GetString(dest);

            }
            return View(usuarioModel);
        }
        #endregion

        #region EDIT - POST

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Models.UsuarioModel usuarioModel)
        {
            Models.UsuarioModel usuario = db.Usuarios.Find(usuarioModel.Id);
            usuarioModel.Senha = usuario.Senha;
            usuarioModel.ConfirmarSenha = usuario.Senha;
            db.Entry(usuario).State = EntityState.Detached;

            //AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            //Convertendo string para byte array
            byte[] src = Encoding.Unicode.GetBytes(usuarioModel.Email);


            //Encriptação
            using (ICryptoTransform decrypt = aes.CreateEncryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                usuarioModel.Email = Convert.ToBase64String(dest);
            }
            db.Entry(usuarioModel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete - GET
        [HttpGet]
        public ActionResult Delete (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Models.UsuarioModel usuarioModel = db.Usuarios.Find(id);

            if (usuarioModel == null)
            {
                return HttpNotFound();
            }

            //AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] src = Convert.FromBase64String(usuarioModel.Email);

            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                usuarioModel.Email = Encoding.Unicode.GetString(dest);
            }

            return View(usuarioModel);

        }
        #endregion

        #region Delete - POST

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)
        {
            Models.UsuarioModel usuarioModel = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarioModel);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("index", "Login");
        }
        #endregion
    }
}