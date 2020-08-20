﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NourishingHands.Utilities
{
    public class SendEmailFromGmail
    {
        public void SendEmail(string toEmail, string toName, string subject, string body, string path = null)
        {
            var fromAddress = new MailAddress("stringbuild1@gmail.com", "Nourishing Hands");
            var toAddress = new MailAddress(toEmail, toName);
            const string fromPassword = "Trinity@2016";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            string html = @"<html><body><img src='cid:NHLogo' style='display: block; margin-left: auto; margin-right: auto; width:225px; height:60px'><hr/><br/>" + body + "</body></html>";
            AlternateView altView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);

            if (path != null)
            {
                LinkedResource yourPictureRes = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                yourPictureRes.ContentId = "NHLogo";
                altView.LinkedResources.Add(yourPictureRes);
            }

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            message.AlternateViews.Add(altView);
            {
                //await smtp.SendMailAsync(message);
                message.IsBodyHtml = true;
                smtp.Send(message);
            }

        }
    }
}
