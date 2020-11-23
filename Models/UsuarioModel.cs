using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Encriptacao.Models
{
    public class UsuarioModel
    {
        internal static string senha;
        internal static string ConfirmaSenha;

        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage ="informe nome do usuario")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="Informe o Email para Login")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage ="Informe a senha:")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength =3,
            ErrorMessage ="Senha mínima 3 caracter")]
        public string Senha { get; set; }

        //[NotMapped]
        [DataType(DataType.Password)]
        [Compare(nameof(Senha), ErrorMessage ="Senha não confere")]
        
        public string ConfirmarSenha { get; set; }

        [Required(ErrorMessage ="Informe o nivel")]

        public  string Nivel { get; set; }



    }
}