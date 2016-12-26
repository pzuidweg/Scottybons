using Mvc.Mailer;

namespace ScottybonsMVC.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            MasterName = "_Layout";
        }

        public virtual MvcMailMessage Welcome(string toAddress, string bodyText)
        {
            //ViewBag.Data = someObject;
            return Populate(msg =>
            {
                msg.Subject = "Welcome";
                msg.ViewName = "Welcome";
                msg.To.Add(toAddress);
                msg.Body = bodyText;
            });
        }

        public virtual MvcMailMessage PasswordReset(string toAddress, string bodyText)
        {
            //ViewBag.Data = someObject;
            return Populate(msg =>
            {
                msg.Subject = "PasswordReset";
                msg.ViewName = "PasswordReset";
                msg.To.Add(toAddress);
                msg.Body = bodyText;
            });
        }

        public MvcMailMessage Welcome()
        {
            //ViewBag.Data = someObject;
            return Populate(msg =>
            {
                msg.Subject = "Welcome";
                msg.ViewName = "Welcome";
                msg.To.Add("srinivas@sidhma.nl");
          
            });
        }

        public MvcMailMessage PasswordReset()
        {
            //ViewBag.Data = someObject;
            return Populate(msg =>
            {
                msg.Subject = "PasswordReset";
                msg.ViewName = "PasswordReset";
            });
        }
    }
}