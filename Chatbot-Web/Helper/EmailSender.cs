using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using System.Diagnostics;

namespace Chatbot_Web.Helper
{
    public static class EmailSender
    {
        public static void SendOtpViaEmail(string email, string code)
        {
            // Create a new instance of MailMessage class
            MailMessage message = new MailMessage();

            // Set subject of the message, body and sender information
            message.Subject = "Verficiation Code";
            message.Body = "Use this Following Code  \n " + code + "\nto Confirm Your Opertaion Kindly Remindrer it's valid for 4 minutes since now";
            message.From = new MailAddress("pcoding3@outlook.com", "Market Store", false);
            // Add To recipients and CC recipients
            message.To.Add(new MailAddress(email, "Recipient 1", false));

            // Create an instance of SmtpClient class
            SmtpClient client = new SmtpClient();

            // Specify your mailing Host, Username, Password, Port # and Security option
            client.Host = "smtp.office365.com";
            client.Username = "pcoding3@outlook.com";
            client.Password = "Z3160@jase";
            client.Port = 587;
            client.SecurityOptions = SecurityOptions.SSLExplicit;
            try
            {
                // Send this email
                client.Send(message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }


        }
    }
}
