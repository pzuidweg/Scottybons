using System.Security.Cryptography;
using Mvc.Mailer;

namespace ScottybonsMVC.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome();
			MvcMailMessage PasswordReset();
            MvcMailMessage Welcome(string to, string body);
            MvcMailMessage PasswordReset(string to, string body);
	}
}