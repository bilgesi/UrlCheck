using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UrlCheck.Data;
using UrlCheck.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace UrlCheck.Controllers
{
    [Authorize]
    public class UrlListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UrlListsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {             
            return View(await _context.UrlLists.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        public async Task<IActionResult> Create(UrlList urlList)
        {
            urlList.CreatedDate = DateTime.Now;
            urlList.CreatedUserEmail = User.Identity.Name.ToString();
            _context.Add(urlList);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await _context.UrlLists.FindAsync(id));
        }

 
        [HttpPost]
        public async Task<IActionResult> Edit(UrlList urlList)
        {

            if (ModelState.IsValid)
            {
                _context.Update(urlList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(urlList);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await _context.UrlLists.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
        
            var urlList = await _context.UrlLists.FindAsync(id);
            if (urlList != null)
            {
                _context.UrlLists.Remove(urlList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UrlCheck(int id)
        {
            ViewBag.urlList = await _context.UrlLists.FindAsync(id);
            return View(await _context.UrlCheckLogs.Where(x => x.UrlListId == id).ToListAsync());
        }

        public async Task<IActionResult> UrlCheckRun(int? id)
        {
            var urlList = await _context.UrlLists.FindAsync(id);
            var urlName = urlList.Url.ToString();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(urlName).Result;

            if (response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;
                _context.UrlCheckLogs.Add(new UrlCheckLog { CreatedDate = DateTime.Now, IsUrlUp = true, UrlListId = urlList.Id, CreatedUserEmail = User.Identity.Name,Url= urlName });
            }
            else
            {
                _context.UrlCheckLogs.Add(new UrlCheckLog { CreatedDate = DateTime.Now, IsUrlUp = false, UrlListId = urlList.Id, CreatedUserEmail = User.Identity.Name, Url = urlName });
                EmailSend(urlList.Email, "Site Hatası", urlList.Url+ " sitesinde hata oluştu");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("UrlCheck", new { id = id });
        }

        private Task EmailSend(string email, string subject, string htmlMessage)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("urlcheck@outlook.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("urlcheck@outlook.com", "Parola123.");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return Task.CompletedTask;
        }

        private bool UrlListExists(int id)
        {
          return _context.UrlLists.Any(e => e.Id == id);
        }
    }
}
