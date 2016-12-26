using System;
using System.Net;
using System.Net.Mail;
using System.Web;
using SendGridMail.Transport;

namespace ScottybonsMVC.Infrastructure
{
    public class EmailHelper
    {
        private const string SEND_GRID_USERNAME = "srmuvvala";
        private const string SEND_GRID_PASSWORD   = "@lt12345";
        private const string EMAIL_FROM_ADDRESS   = "srinivas@sidhma.com";

        public void SendResetPasswordEmail(string memberEmail, string resetGuid)
        {
            //Send a reset email to member
            // Create the email object first, then add the properties.
            var myMessage = SendGridMail.SendGrid.GetInstance();

            // Add the message properties.
            myMessage.From = new MailAddress(EMAIL_FROM_ADDRESS);

            //Send to the member's email address
            myMessage.AddTo(memberEmail);

            //Subject
            myMessage.Subject = "Umb Jobs - Reset Your Password";

            //Reset link
            string baseUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var resetUrl = baseUrl + "/resetpassword?resetGUID=" + resetGuid;

            //HTML Message
            myMessage.Html = string.Format(
                                "<h3>Reset Your Password</h3>" +
                                "<p>You have requested to reset your password<br/>" +
                                "If you have not requested to reste your password, simply ignore this email and delete it</p>" +
                                "<p><a href='{0}'>Reset your password</a></p>",
                                resetUrl);


            //PlainText Message
            myMessage.Text = string.Format(
                                "Reset your password" + Environment.NewLine +
                                "You have requested to reset your password" + Environment.NewLine +
                                "If you have not requested to reste your password, simply ignore this email and delete it" +
                                Environment.NewLine + Environment.NewLine +
                                "Reset your password: {0}",
                                resetUrl);

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(SEND_GRID_USERNAME, SEND_GRID_PASSWORD);

            // Create an SMTP transport for sending email.
            var transportSmtp = SMTP.GetInstance(credentials);

            // Send the email.
            transportSmtp.Deliver(myMessage);
        }

        public void SendVerifyEmail(string memberEmail, string verifyGuid)
        {
            //Send a reset email to member
            // Create the email object first, then add the properties.
            var myMessage = SendGridMail.SendGrid.GetInstance();

            // Add the message properties.
            myMessage.From = new MailAddress(EMAIL_FROM_ADDRESS);

            //Send to the member's email address
            myMessage.AddTo(memberEmail);

            //Subject
            myMessage.Subject = "Umb Jobs - Verify Your Email";

            //Verify link
            string baseURL = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var verifyURL = baseURL + "/verifyemail?verifyGUID=" + verifyGuid;

            //HTML Message
            myMessage.Html = string.Format(
                                "<h3>Verify Your Email</h3>" +
                                "<p>Click here to verify your email address and active your account today</p>" +
                                "<p><a href='{0}'>Verify your email & active your account</a></p>",
                                verifyURL);


            //PlainText Message
            myMessage.Text = string.Format(
                                "Verify Your Email" + Environment.NewLine +
                                "Click here to verify your email address and active your account today" + Environment.NewLine +
                                Environment.NewLine + Environment.NewLine +
                                "{0}",
                                verifyURL);

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(SEND_GRID_USERNAME, SEND_GRID_PASSWORD);

            // Create an SMTP transport for sending email.
            var transportSmtp = SMTP.GetInstance(credentials);

            // Send the email.
            transportSmtp.Deliver(myMessage);
        }
    }
}