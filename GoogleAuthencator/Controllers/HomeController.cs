using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Authenticator;
namespace GoogleAuthencator.Controllers
{
    public class HomeController : Controller
    {
        private string secretKey = "TUANITPRODOTCOM_GoogleAuthencator";
        public ActionResult Index()
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            tfa.UseManagedSha1Algorithm = true;

            string title = "GoogleAuthencator";
            
            var setupInfo = tfa.GenerateSetupCode(title, "demo@email.com", secretKey, 300, 300);
            string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            string manualEntrySetupCode = setupInfo.ManualEntryKey;

            ViewBag.QrCodeImageUrl = qrCodeImageUrl;
            ViewBag.ManualEntrySetupCode = manualEntrySetupCode;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string txtYourCode)
        {
            var result = "Not OK";
            if (!string.IsNullOrEmpty(txtYourCode))
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                tfa.DefaultClockDriftTolerance = TimeSpan.FromSeconds(30);
                tfa.UseManagedSha1Algorithm = true;

                bool isCorrectPIN = tfa.ValidateTwoFactorPIN(secretKey, txtYourCode);
                if (isCorrectPIN)
                {
                    result = "OK";
                }
                else
                {
                    result = "2FA is not a valid PIN";
                }
            }
            TempData["Authen"] = result;
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}