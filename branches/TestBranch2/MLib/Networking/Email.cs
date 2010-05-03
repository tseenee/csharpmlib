using System.Web.Mail;

namespace MLib.Networking
{
    class Email
    {
        public static void SendEmail()
        {
            SmtpMail.SmtpServer = "smtp.mail.com";

            string from = "jondoe@bla.net";

            string to = "justme@isp.com";

            string subject = "voila";

            string body = "sdfsdfsdf, yes, it's all about sdfsdfsdf.";

            SmtpMail.Send(from, to, subject, body);
        }
    }
}
