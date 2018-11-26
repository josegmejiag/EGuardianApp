using System;
namespace EGuardian.Models.Contrasenia
{
    public class ResetPassword
    {
        public string email { get; set; }
        public string parametros
        {
            get
            {
                return
                    "email=" + email;
            }
        }
    }

    public class ChangePassword
    {
        public string newPassword { get; set; }
        public string oldPassword { get; set; }
    }

    public class ValidarToken
    {
        public string newPassword { get; set; }
        public string token { get; set; }
    }

    public class ResetPasswordResponse
    {
        public string result { get; set; }
        public string token { get; set; }
    }

    public class ValidarTokenResponse
    {
        public string result { get; set; }
    }
}