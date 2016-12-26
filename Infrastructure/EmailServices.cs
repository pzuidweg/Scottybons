using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using ScottybonsMVC.AppConstants;
using ScottybonsMVC.Models.Entities;


namespace ScottybonsMVC.Infrastructure
{
    /// <summary>
    /// Email Service
    /// </summary>
    public class EmailServices
    {
        readonly ScottybonsEComEntities _scottybonsEComEntities;
        public EmailServices()
        {

        }

        public EmailServices(ScottybonsEComEntities scottybonsEComEntities)
        {
            _scottybonsEComEntities = scottybonsEComEntities;
        }


        /// <summary>
        /// Send Email By Content Type
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="customerName"></param>
        /// <param name="toAddress"></param>
        /// <param name="orderNumber"></param>
        public void SendEmailWithOutLink(int emailType, string customerName, string toAddress, string orderNumber)
        {
            try
            {
                var emailContent =
               _scottybonsEComEntities.EmailContents.FirstOrDefault(
                   em =>
                       em.EmailType.Equals((int)emailType) &&
                       em.EmailContentLang.Trim().ToLower().Equals(GlobalConstants.Language));


                string path = GetEmailContent(emailType);
                if (string.IsNullOrEmpty(path))
                    return;
                string content = System.IO.File.ReadAllText(path);


                //Send a reset email to member
                // Create the email object first, then add the properties.
                //var myMessage = SendGridMail.SendGrid.GetInstance();

                var myMessage = new MailMessage();



                var orderString = " (" + Resources.Scottybons.ScottybonsDataStrings.ThankYouOrderWithOrderNumber.Replace("*|ORDNUM|*", orderNumber) + ") ";
                //Subject
                //myMessage.Subject = emailContent.Subject + OrderString;

                var body = content;//emailContent.EmailHTMLFormat;
                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);
                body = body.Replace("*|FNAME|*", customerName);
                body = body.Replace("*|HOMEURL|*", ConfigurationManager.AppSettings["HomeUrl"]);
                body = body.Replace("*|HOMEURLIMAGES|*", ConfigurationManager.AppSettings["HOMEURLIMAGES"]);
                body = body.Replace("*|HOMEFONTURL|*", ConfigurationManager.AppSettings["HOMEFONTURL"]);
                body = body.Replace("*|HOMEURLFORCSS|*", ConfigurationManager.AppSettings["HOMEURLFORCSS"]);
                body = body.Replace("*|SUBJECT|*", emailContent.Subject);
                body = body.Replace("*|BODYMIDDLE|*", emailContent.BodyMiddle);
                body = body.Replace("*|ORDNUM|*", orderNumber);
                body = body.Replace("*|CLOSING|*", emailContent.Closing);
                body = body.Replace("*|ENDNOTE|*", emailContent.EndNote);

                //TO Address
                myMessage.To.Add(toAddress);
                //From
                myMessage.From = new MailAddress(GlobalConstants.ScottybonEmailAddress);
                //Subject
                myMessage.Subject = emailContent.Subject + orderString;

                //Main content Body
                myMessage.Body = body;

                //HTML Message (true) -  //PlainText Message (false)
                myMessage.IsBodyHtml = true;


                var smtpClient = new SmtpClient();

                smtpClient.Host = GlobalConstants.SmtpHost;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                // Create credentials, specifying your user name and password.
                smtpClient.Credentials = new NetworkCredential(GlobalConstants.SmtpUserName, GlobalConstants.SmtpUPassword);

                //Send Email
                smtpClient.Send(myMessage);
            }
            catch (Exception ex)
            {
                //Throw Exception
                throw;
            }

        }

        /// <summary>
        /// Send Email with the Link
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="customerName"></param>
        /// <param name="toAddress"></param>
        /// <param name="navigationLink"></param>
        /// <param name="orderNumber"></param>
        public void SendEmailWithLink(int emailType, string customerName, string toAddress,string navigationLink = null, string orderNumber = "")
        {
            try
            {
                var emailContent =
               _scottybonsEComEntities.EmailContents.FirstOrDefault(
                   em =>
                       em.EmailType.Equals((int)emailType) &&
                       em.EmailContentLang.Trim().ToLower().Equals(GlobalConstants.Language));


                string path = GetEmailContent(emailType);
                if (string.IsNullOrEmpty(path))
                    return;
                string content = System.IO.File.ReadAllText(path);

                //Send a reset email to member
                // Create the email object first, then add the properties.
                var myMessage = new MailMessage();

                //Subject
                myMessage.Subject = emailContent.Subject.Trim();

                var body = content;
                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);
                body = body.Replace("*|FNAME|*", customerName);
                body = body.Replace("*|HOMEURL|*", ConfigurationManager.AppSettings["HomeUrl"]);
                body = body.Replace("*|HOMEURLIMAGES|*", ConfigurationManager.AppSettings["HOMEURLIMAGES"]);
                body = body.Replace("*|HOMEFONTURL|*", ConfigurationManager.AppSettings["HOMEFONTURL"]);
                body = body.Replace("*|HOMEURLFORCSS|*", ConfigurationManager.AppSettings["HOMEURLFORCSS"]);
                body = body.Replace("*|SUBJECT|*", emailContent.Subject);
                body = body.Replace("*|BODYMIDDLE|*", emailContent.BodyMiddle);
                body = body.Replace("*|CLOSING|*", emailContent.Closing);
                body = body.Replace("*|ENDNOTE|*", emailContent.EndNote);
                body = body.Replace("*|ORDNUM|*", orderNumber);
                // if (!ReferenceEquals(navigationLink, null))
                // body = body.Replace("*|LINK|*", string.Format("<a href='{0}'>" + Resources.Scottybons.ScottybonsDataStrings.LinkClickHere + "</a>", navigationLink));
                body = body.Replace("*|LINK|*",  navigationLink);




                //TO Address
                myMessage.To.Add(toAddress);
                //From
                myMessage.From = new MailAddress(GlobalConstants.ScottybonEmailAddress);

                //Main content Body
                myMessage.Body = body;

                //HTML Message (true) -  //PlainText Message (false)
                myMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient();

                smtpClient.Host = GlobalConstants.SmtpHost;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                // Create credentials, specifying your user name and password.
                smtpClient.Credentials = new NetworkCredential(GlobalConstants.SmtpUserName, GlobalConstants.SmtpUPassword);

                //Send Email
                smtpClient.Send(myMessage);
            }
            catch (Exception ex)
            {

                // throw;
            }

        }


        #region "Commented Code"
        ///// <summary>
        ///// Send Email
        ///// </summary>
        ///// <param name="toAddress"></param>
        ///// <param name="subject"></param>
        ///// <param name="fromAddress"></param>
        ///// <param name="body"></param>
        ///// <param name="sMtpHost"></param>
        ///// <param name="smtpUserName"></param>
        ///// <param name="smtpPassword"></param>
        ///// <param name="smtpPort"></param>
        //public void SendEmail(string toAddress, string subject, string fromAddress, string body, string sMtpHost, string smtpUserName, string smtpPassword, int smtpPort = 587)
        //{
        //    try
        //    {
        //        var message = new System.Net.Mail.MailMessage();

        //        var myMessage = SendGridMail.SendGrid.GetInstance();

        //        message.To.Add(toAddress); //recipient
        //        message.Subject = subject;
        //        message.From = new System.Net.Mail.MailAddress(fromAddress); //from email
        //        message.Body = body;
        //        message.IsBodyHtml = true;
        //        var smtp = new System.Net.Mail.SmtpClient(sMtpHost)
        //        {
        //            Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword),
        //            //EnableSsl = true,
        //            UseDefaultCredentials = false,
        //            DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
        //            Port = smtpPort
        //        };  //  need an smtp server address to send emails
        //        smtp.EnableSsl = true;
        //        smtp.Send(message);
        //        // umbraco.library.SendMail(fromAddress, toAddress, subject, body, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        //Swallo Exception
        //        //throw ex;
        //    }


        //}


        ///// <summary>
        ///// Send Email
        ///// </summary>
        ///// <param name="toAddress"></param>
        ///// <param name="subject"></param>
        ///// <param name="fromAddress"></param>
        ///// <param name="body"></param>
        ///// <param name="smtpUserName"></param>
        ///// <param name="smtpPassword"></param>
        ///// <param name="smtpPort"></param>
        //public void SendEmailUsingSendGrid(string toAddress, string subject, string fromAddress, string body, string smtpUserName, string smtpPassword, int smtpPort = 587)
        //{
        //    try
        //    {
        //        //Send a reset email to member
        //        // Create the email object first, then add the properties.
        //        var myMessage = SendGridMail.SendGrid.GetInstance();

        //        // Add the message properties.
        //        myMessage.From = new MailAddress(fromAddress);

        //        //Send to the member's email address
        //        myMessage.AddTo(toAddress);

        //        //Subject
        //        myMessage.Subject = subject;

        //        //HTML Message
        //        myMessage.Html = string.Format(body);


        //        //PlainText Message
        //        myMessage.Text = string.Format(body);

        //        // Create credentials, specifying your user name and password.
        //        var credentials = new NetworkCredential(smtpUserName, smtpPassword);

        //        // Create an SMTP transport for sending email.
        //        var transportSmtp = SMTP.GetInstance(credentials);

        //        // Send the email.
        //        transportSmtp.Deliver(myMessage);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
        #endregion

        /// <summary>
        /// Send Email By Content Type
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="customerName"></param>
        /// <param name="toAddress"></param>
        public void SendEmailByContentType(int emailType, string customerName, string toAddress)
        {
            try
            {
                var emailContent =
               _scottybonsEComEntities.EmailContents.FirstOrDefault(
                   em =>
                       em.EmailType.Equals((int)emailType) &&
                       em.EmailContentLang.Trim().ToLower().Equals(GlobalConstants.Language));


                string path = GetEmailContent(emailType);
                if (string.IsNullOrEmpty(path))
                    return;
                string content = System.IO.File.ReadAllText(path);

                //Send a reset email to member

                var myMessage = new MailMessage();
                //Subject
                myMessage.Subject = emailContent.Subject;

                var body = content;//emailContent.EmailHTMLFormat;
                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);
                body = body.Replace("*|FNAME|*", customerName);
                body = body.Replace("*|HOMEURL|*", ConfigurationManager.AppSettings["HomeUrl"]);
                body = body.Replace("*|HOMEURLIMAGES|*", ConfigurationManager.AppSettings["HOMEURLIMAGES"]);
                body = body.Replace("*|HOMEFONTURL|*", ConfigurationManager.AppSettings["HOMEFONTURL"]);
                body = body.Replace("*|HOMEURLFORCSS|*", ConfigurationManager.AppSettings["HOMEURLFORCSS"]);
                body = body.Replace("*|SUBJECT|*", emailContent.Subject);
                body = body.Replace("*|BODYMIDDLE|*", emailContent.BodyMiddle);
                body = body.Replace("*|CLOSING|*", emailContent.Closing);
                body = body.Replace("*|ENDNOTE|*", emailContent.EndNote);


                //TO Address
                myMessage.To.Add(toAddress);
                //From
                myMessage.From = new MailAddress(GlobalConstants.ScottybonEmailAddress);

                //Main content Body
                myMessage.Body = body;

                //HTML Message (true) -  //PlainText Message (false)
                myMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient();

                smtpClient.Host = GlobalConstants.SmtpHost;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                // Create credentials, specifying your user name and password.
                smtpClient.Credentials = new NetworkCredential(GlobalConstants.SmtpUserName, GlobalConstants.SmtpUPassword);

                //Send Email
                smtpClient.Send(myMessage);

            }
            catch (Exception ex)
            {

                // throw;
            }

        }


        /// <summary>
        /// Send Email By Content Type
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="customerName"></param>
        /// <param name="toAddress"></param>
        /// <param name="resetLink"></param>
        public void SendEmailForForgotPassword(int emailType, string customerName, string toAddress, string resetLink)
        {
            try
            {
                var emailContent =
               _scottybonsEComEntities.EmailContents.FirstOrDefault(
                   em =>
                       em.EmailType.Equals((int)emailType) &&
                       em.EmailContentLang.Trim().ToLower().Equals(GlobalConstants.Language));


                string path = GetEmailContent(emailType);
                if (string.IsNullOrEmpty(path))
                    return;
                string content = System.IO.File.ReadAllText(path);

                //Send a reset email to member
                // Create the email object first, then add the properties.
                var myMessage = new MailMessage();

                //Subject
                myMessage.Subject = emailContent.Subject;

                var body = content;
                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);
                body = body.Replace("*|FNAME|*", customerName);
                body = body.Replace("*|HOMEURL|*", ConfigurationManager.AppSettings["HomeUrl"]);
                body = body.Replace("*|HOMEURLIMAGES|*", ConfigurationManager.AppSettings["HOMEURLIMAGES"]);
                body = body.Replace("*|HOMEFONTURL|*", ConfigurationManager.AppSettings["HOMEFONTURL"]);
                body = body.Replace("*|HOMEURLFORCSS|*", ConfigurationManager.AppSettings["HOMEURLFORCSS"]);
                body = body.Replace("*|SUBJECT|*", emailContent.Subject);
                body = body.Replace("*|BODYMIDDLE|*", emailContent.BodyMiddle);
                body = body.Replace("*|CLOSING|*", emailContent.Closing);
                body = body.Replace("*|ENDNOTE|*", emailContent.EndNote);

                body = body.Replace("*|LINK|*", string.Format("<a href='{0}'>" + Resources.Scottybons.ScottybonsDataStrings.ClickHere + "</a>", resetLink));

                //TO Address
                myMessage.To.Add(toAddress);

                //From
                myMessage.From = new MailAddress(GlobalConstants.ScottybonEmailAddress);

                //Main content Body
                myMessage.Body = body;

                //HTML Message (true) -  //PlainText Message (false)
                myMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient();

                smtpClient.Host = GlobalConstants.SmtpHost;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;

                // Create credentials, specifying your user name and password.
                smtpClient.Credentials = new NetworkCredential(GlobalConstants.SmtpUserName, GlobalConstants.SmtpUPassword);

                //Send Email
                smtpClient.Send(myMessage);
            }
            catch (Exception ex)
            {

                // throw;
            }

        }


        /// <summary>
        /// Send Email By Content Type
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="contactUsContent"></param>
        /// <param name="customerAddress"></param>
        public void SendEmailForSupportAdmin(int emailType, string contactUsContent, string customerAddress)
        {
            try
            {
                var emailContent =
               _scottybonsEComEntities.EmailContents.FirstOrDefault(
                   em =>
                       em.EmailType.Equals((int)emailType) &&
                       em.EmailContentLang.Trim().ToLower().Equals(GlobalConstants.Language));


                string path = GetEmailContent(emailType);
                if (string.IsNullOrEmpty(path))
                    return;
                string content = System.IO.File.ReadAllText(path);

                //Send a reset email to member
                // Create the email object first, then add the properties.
                var myMessage = new MailMessage();

                //Subject
                myMessage.Subject = emailContent.Subject;


                //Subject
                myMessage.Subject = emailContent.Subject;

                var body = content;
                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);
                body = body.Replace("*|FNAME|*", GlobalConstants.ScottybonCustomerSupportAdminName);
                body = body.Replace("*|HOMEURL|*", ConfigurationManager.AppSettings["HomeUrl"]);
                body = body.Replace("*|HOMEURLIMAGES|*", ConfigurationManager.AppSettings["HOMEURLIMAGES"]);
                body = body.Replace("*|HOMEFONTURL|*", ConfigurationManager.AppSettings["HOMEFONTURL"]);
                body = body.Replace("*|HOMEURLFORCSS|*", ConfigurationManager.AppSettings["HOMEURLFORCSS"]);
                body = body.Replace("*|SUBJECT|*", emailContent.Subject);
                body = body.Replace("*|BODYMIDDLE|*", contactUsContent);
                body = body.Replace("*|CLOSING|*", emailContent.Closing);
                body = body.Replace("*|ENDNOTE|*", emailContent.EndNote);


                //TO Address
                myMessage.To.Add(GlobalConstants.ScottybonEmailToAddress);

                //From
                myMessage.From = new MailAddress(GlobalConstants.ScottybonEmailAddress);

                //Main content Body
                myMessage.Body = body;

                //HTML Message (true) -  //PlainText Message (false)
                myMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient();

                smtpClient.Host = GlobalConstants.SmtpHost;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;

                // Create credentials, specifying your user name and password.
                smtpClient.Credentials = new NetworkCredential(GlobalConstants.SmtpUserName, GlobalConstants.SmtpUPassword);

                //Send Email
                smtpClient.Send(myMessage);
            }
            catch (Exception ex)
            {
                //throw;
            }

        }

        /// <summary>
        /// Send Email with the Link
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="customerName"></param>
        /// <param name="customerLastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="createdTime"></param>
        /// <param name="styleFormQuestionsAnswers"></param>
        /// <param name="createdDate"></param>
        public void SendEmailForIntake(int emailType, string customerName, string customerLastName, string emailAddress, string toAddress, string createdDate, string createdTime, string styleFormQuestionsAnswers)
        {
            try
            {
                var emailContent =
               _scottybonsEComEntities.EmailContents.FirstOrDefault(
                   em =>
                       em.EmailType.Equals((int)emailType) &&
                       em.EmailContentLang.Trim().ToLower().Equals(GlobalConstants.Language));

                string path = GetEmailContent(emailType);

                if (string.IsNullOrEmpty(path))
                    return;
                string content = System.IO.File.ReadAllText(path);

                //Send a reset email to member
                // Create the email object first, then add the properties.
                var myMessage = new MailMessage();

                //Subject
                myMessage.Subject = emailContent.Subject.Trim();

                var body = content;
                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);
                body = body.Replace("*|BODYMIDDLE|*", emailContent.BodyMiddle);
                body = body.Replace("*|FNAME|*", GlobalConstants.ScottybonCustomerSupportAdminName);

                body = body.Replace("*|SLNAME|*", customerLastName);
                body = body.Replace("*|SFNAME|*", customerName);
                body = body.Replace("*|DATE|*", createdDate);
                body = body.Replace("*|TIME|*", createdTime);
                body = body.Replace("*|EMAIL|*", emailAddress);
                body = body.Replace("*|DETAILS|*", styleFormQuestionsAnswers);
                body = body.Replace("*|HOMEURL|*", ConfigurationManager.AppSettings["HomeUrl"]);
                body = body.Replace("*|HOMEURLIMAGES|*", ConfigurationManager.AppSettings["HOMEURLIMAGES"]);
                body = body.Replace("*|HOMEFONTURL|*", ConfigurationManager.AppSettings["HOMEFONTURL"]);
                body = body.Replace("*|HOMEURLFORCSS|*", ConfigurationManager.AppSettings["HOMEURLFORCSS"]);
                body = body.Replace("*|SUBJECT|*", emailContent.Subject);
                body = body.Replace("*|CLOSING|*", emailContent.Closing);
                body = body.Replace("*|ENDNOTE|*", emailContent.EndNote);


                //TO Address
                myMessage.To.Add(toAddress);
                //From
                myMessage.From = new MailAddress(GlobalConstants.ScottybonEmailAddress);

                //Main content Body
                myMessage.Body = body;

                //HTML Message (true) -  //PlainText Message (false)
                myMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient();

                smtpClient.Host = GlobalConstants.SmtpHost;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                // Create credentials, specifying your user name and password.
                smtpClient.Credentials = new NetworkCredential(GlobalConstants.SmtpUserName, GlobalConstants.SmtpUPassword);

                //Send Email
                smtpClient.Send(myMessage);
            }
            catch (Exception ex)
            {

                // throw;
            }

        }



        public void SendOrderNotificationToAdmin(int emailType, Customer customer, Order order, string toAddress)
        {
            try
            {
                var emailContent =
               _scottybonsEComEntities.EmailContents.FirstOrDefault(
                   em =>
                       em.EmailType.Equals((int)emailType) &&
                       em.EmailContentLang.Trim().ToLower().Equals(GlobalConstants.Language));

                string path = GetEmailContent(emailType);

                if (string.IsNullOrEmpty(path))
                    return;
                string content = System.IO.File.ReadAllText(path);

                //Send a reset email to member
                // Create the email object first, then add the properties.
                var myMessage = new MailMessage { Subject = emailContent.Subject.Trim() };



                TimeZoneInfo wEuropeStandardTimeZoneInfo =
                    TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

                DateTime wEuropeStandardTime = TimeZoneInfo.ConvertTimeFromUtc(order.CreatedOn,
                    wEuropeStandardTimeZoneInfo);



                var body = content;

                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);
                body = body.Replace("*|INTRO|*", emailContent.Intro);
                body = body.Replace("*|BODYMIDDLE|*", emailContent.BodyMiddle);
                body = body.Replace("*|FNAME|*", "<b>" + customer.FirstName + "</b>");
                body = body.Replace("*|LNAME|*", "<b>" + customer.LastName + "</b>");

                body = body.Replace("*|DATE|*", "<b>" + wEuropeStandardTime.ToShortDateString() + "</b>");
                body = body.Replace("*|TIME|*", "<b>" + wEuropeStandardTime.ToShortTimeString() + "</b>");
                body = body.Replace("*|ORDNUM|*", "<b>" + order.OrderID.ToString() + "</b>");
                body = body.Replace("*|EMAIL|*", "<b>" + customer.Email + "</b>");

                var objOcc =
                    _scottybonsEComEntities.OccasionForScottyBoxMasters.FirstOrDefault(
                        oc => oc.OccasionForScottyBoxID == order.OrderOccasion);

                var objPer =
                    _scottybonsEComEntities.PeriodicalScottyBoxMasters.FirstOrDefault(
                        oc => oc.PeriodicalScottyBoxID == order.PeriodicalScottyBoxID);

                var objCountry = _scottybonsEComEntities.CountryMasters.FirstOrDefault(oc => oc.CountryID == order.OrderCountryID);

                body = !ReferenceEquals(objCountry, null)
                    ? body.Replace("*|COUNTRY|*",
                        "<b>" +
                        objCountry.CountryName + "</b>" + "<br>")
                    : body.Replace("*|COUNTRY|*",
                        "<b>" + "No Country Selected" + "</b>" + "<br>");

                var objOrderStatus =
                    _scottybonsEComEntities.OrderStatusMasters.FirstOrDefault(
                        oc => oc.OrderStatusID == order.OrderStatusID);

                body = !ReferenceEquals(objCountry, null) ? body.Replace("*|PAYSTATUS|*", "<b>" + _scottybonsEComEntities.OrderStatusMasters.FirstOrDefault(oc => oc.OrderStatusID == order.OrderStatusID).OrderStatusName + "</b>") : body.Replace("*|PAYSTATUS|*", "<b>" + "Order Created" + "</b>");


                body = !ReferenceEquals(objPer, null) ? body.Replace("*|PERIODFORSUBSCRIPION|*", "<b>" + objPer.PerodicalMonths + "</b>" + "<br>") : body.Replace("*|PERIODFORSUBSCRIPION|*", "<b>" + "No Periodic Subscription" + "</b>" + "<br>");

                body = !ReferenceEquals(objPer, null) ? body.Replace("*|SUBSCRIPTION|*", "<b>" + objPer.PeriodicalScottyBox + "</b>" + "<br>") : body.Replace("*|SUBSCRIPTION|*", "<b>" + "Not Subscribed " + "</b>" + "<br>");

                body = !ReferenceEquals(objOcc, null) ? body.Replace("*|OCCASION|*", "<b>" + objOcc.OccasionForScottyBoxName + "</b>" + "<br>") : body.Replace("*|OCCASION|*", "<b>" + "No Occasion Selected " + "</b>" + "<br>");

                body = body.Replace("*|TELEPHONE|*", ReferenceEquals(order.ContactNumber, null) ? "<b>" + "No Phone Number" + "</b>" + "<br>" : "<b>" + order.ContactNumber + "</b>" + "<br>");
                body = body.Replace("*|STREETNAME|*", "<b>" + order.OrderStreet + "</b>" + "<br>");
                body = body.Replace("*|STREETNUMBER|*", "<b>" + order.OrderHouseNumber + "</b>" + "<br>");
                body = body.Replace("*|POSTCODE|*", "<b>" + order.OrderPostalCode + "</b>" + "<br>");

                body = body.Replace("*|TOWN|*", "<b>" + order.OrderTown + "</b><br>");
                if (order.DeliverNeighbours.HasValue)
                {
                    var deliverNeighbours = (order.DeliverNeighbours == true ? "Yes" : "No");
                    body = body.Replace("*|PERMISSIONTODELIVERATNEIGHBOURS|*", "<b>" + deliverNeighbours + "</b>" + "<br>");
                }
                else
                {
                    body = body.Replace("*|PERMISSIONTODELIVERATNEIGHBOURS|*", "<b>" + "No" + "</b>" + "<br>");
                }

                if (order.IsStylistContactYou.HasValue)
                {
                    var isStylistContactYou = (order.IsStylistContactYou == true ? "Yes" : "No");
                    body = body.Replace("*|PERMISSIONTOCALL|*", "<b>" + isStylistContactYou.ToString() + "</b>" + "<br>");
                    if (isStylistContactYou == "Yes")
                    {
                        body = body.Replace("*|TELEPHONE|*", ReferenceEquals(order.ContactNumber, null) ? "<b>" + "No Phone Number" + "</b>" + "<br>" : "<b>" + order.ContactNumber + "</b>" + "<br>");
                    }
                    else
                    {
                        body = body.Replace("*|TELEPHONE|*",   "<b>" + " " + "</b>" + "</b>" + "<br>");
                    }
                }
                else
                {
                    body = body.Replace("*|PERMISSIONTOCALL|*", "<b>" + "No" + "</b>" + "<br>");
                    body = body.Replace("*|TELEPHONE|*", "<b>" + " " + "</b>" + "</b>" + "<br>");
                }

                //SB-16


                body = body.Replace("*|HOMEURLFORLOGO|*", ConfigurationManager.AppSettings["HOMEURLFORLOGO"]);

                body = body.Replace("*|HOMEURL|*", ConfigurationManager.AppSettings["HomeUrl"]);
                body = body.Replace("*|HOMEURLIMAGES|*", ConfigurationManager.AppSettings["HOMEURLIMAGES"]);
                body = body.Replace("*|HOMEFONTURL|*", ConfigurationManager.AppSettings["HOMEFONTURL"]);

                body = body.Replace("*|HOMEURLFORCSS|*", ConfigurationManager.AppSettings["HOMEURLFORCSS"]);
                body = body.Replace("*|SUBJECT|*", emailContent.Subject);
                body = body.Replace("*|CLOSING|*", emailContent.Closing);
                body = body.Replace("*|ENDNOTE|*", emailContent.EndNote);



                //TO Address
                myMessage.To.Add(toAddress);
                //From
                myMessage.From = new MailAddress(GlobalConstants.ScottybonEmailAddress);

                //Main content Body
                myMessage.Body = body;

                //HTML Message (true) -  //PlainText Message (false)
                myMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient();

                smtpClient.Host = GlobalConstants.SmtpHost;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                // Create credentials, specifying your user name and password.
                smtpClient.Credentials = new NetworkCredential(GlobalConstants.SmtpUserName, GlobalConstants.SmtpUPassword);

                //Send Email
                smtpClient.Send(myMessage);
            }
            catch (Exception ex)
            {

                // throw;
            }

        }


        /// <summary>
        /// Email Content 
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetEmailContent(int emailType, string path = "")
        {
            if (emailType.Equals((int)EmailReason.OrderNotificationMail) || emailType.Equals((int)EmailReason.PlacesAnOrder))
            {
                path = GlobalConstants.EmailTemplatePath;
            }
            else if (emailType.Equals((int)EmailReason.RegistersAndCreatesAnAccount))
            {
                path = GlobalConstants.EmailTemplatePath10;
            }
            else if (emailType.Equals((int)EmailReason.ForgotPassword) || emailType.Equals((int)EmailReason.FillsStyleProfile) || emailType.Equals((int)EmailReason.ChangesStyleProfile) || emailType.Equals((int)EmailReason.PaysForStyleAdvice) || emailType.Equals((int)EmailReason.PaymentFailure))
            {
                path = GlobalConstants.EmailTemplatePath30;
            }
            else
            {
                path = GlobalConstants.EmailTemplatePath30;
            }
            return path;
        }
    }
}