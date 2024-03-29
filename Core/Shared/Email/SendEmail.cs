﻿using System.Net.Mail;

namespace Core.Shared.Email;

public class SendEmail : IMailSender
{
    public void Send(string to, string subject, string body)
    {
        var defaultEmail = "saeedmailsender@gmail.com";
        var mail = new MailMessage();
        var SmtpServer = new SmtpClient("smtp.gmail.com");
        mail.From = new MailAddress(defaultEmail, "تست ارسال ایمیل فعالسازی");
        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        // System.Net.Mail.Attachment attachment;
        // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
        // mail.Attachments.Add(attachment);

        SmtpServer.Port = 587;
        // بر اسا آخرین آپدیت گوگل بر اساس قابلیت 
        // app paswords 
        // این پسورد مخصوص استفاده در این سیستم است
        SmtpServer.Credentials = new System.Net.NetworkCredential(defaultEmail, "qcxwgexoiqosbent");
        SmtpServer.EnableSsl = true;


        //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //SmtpServer.UseDefaultCredentials = false;

        SmtpServer.Send(mail);
    }
}