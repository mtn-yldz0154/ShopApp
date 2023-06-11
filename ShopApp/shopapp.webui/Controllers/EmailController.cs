using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    public class EmailController:Controller
    {
     public IActionResult Index()
    {
        return View();
    }

        [HttpPost]
    public IActionResult Index(EmailModel model)
    {
        using (MailMessage mm = new MailMessage(model.Email, model.To))
        {
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            if (model.Attachment.Length > 0)
            {
                string fileName = Path.GetFileName(model.Attachment.FileName);
                mm.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(),fileName));
            }
            mm.IsBodyHtml = false;
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                ViewBag.Message = "Email sent.";
            }
        }
 
        return View();
    }
        
    }
}