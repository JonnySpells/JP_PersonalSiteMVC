using JP_PersonalSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace JP_PersonalSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Contact()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Contact(ContactViewModels cvm)
        {
            //When processing form data in post action, ALWAYS check validation FIRST
            if (!ModelState.IsValid)//if ModelState IS NOT valid
            {
                //Send them back to the form with error messages to let them know what to fix.
                //Those error messages are populated in the <span> tags in the View - and this happens automatically
                //because out view is bound to our Model
                return View(cvm);
            }

            //If we make it here, validation has been passed and we can process sending the email.
            //first, we'll need another NuGet Package in order to send the email
            #region Installing MimeKit for email
            /*
             * 1) Open the package manager console
             *  - Tools -> NuGet package manager -> Package Manager Console
             * 2) Type install-package NETCore.MailKit
             *  - Press Enter
             * 3) Add using MimeKit;
             * 4) Add using MailKit.Net.Smtp;
             * **/
            #endregion

            //**** Create the MimeMessage object (AKA the email) ****//
            //create a variable for the body of the message
            string message = $"Hello! You have received a new email from your website's contact form! <br />" +
                $"Sender: {cvm.Name}<br />Email: {cvm.Email}<br />Subject: {cvm.Subject}<br />Message: {cvm.Message}<br /><br />" +
                $"<strong>DO NOT REPLY TO THIS EMAIL. Send replies to {cvm.Email}</strong>";

            //Create an instance of MimeMessage that will store all of the email objects info
            var msg = new MimeMessage();

            //Assign Credentials to send the email
            msg.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            msg.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));
            //COMMENT BELOW OUT AFTER TESTING
            //msg.Cc.Add(new MailboxAddress("Cc", "ewhittaker@centriq.com"));
            //msg.Cc.Add(new MailboxAddress("Cc", "jcaldwell@centriq.com"));
            //msg.Cc.Add(new MailboxAddress("Cc", "jdemaranville@centriq.com"));

            msg.Subject = cvm.Subject;

            msg.Body = new TextPart("HTML") { Text = message };

            //OPTIONAL: set the message priority
            msg.Priority = MessagePriority.Urgent;

            //Attempt to send the email
            using (var client = new SmtpClient())
            {
                //Connect to the mail server
                client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                //Authenticate our email user
                client.Authenticate(
                    //Username
                    _config.GetValue<string>("Credentials:Email:User"),

                    //Password
                    _config.GetValue<string>("Credentials:Email:Password")
                    );

                try
                {
                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    //If for any reason the client.send(msg) fails, this code will execute and allow us to exit gracefully
                    //without causing a runtime exception
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please try again later.<br />" +
                        $"Error Message: {ex.StackTrace}";

                    //Return the user to the view with the information they supplied in the form
                    return View(cvm);
                }

            }//End using SMTPclient

            //If all goes well, return a view that displays a confirmation of the message being sent
            return View("EmailConfirmation", cvm);

            //Email - Step 10
            //Add the EmailConfirmation view
            // - Right click here in the Action -> Add View
            // - Select Razor View - Empty
            // - Name: EmailConfirmation
            // - Add the @model declaration to the View
            // - Code the view's HTML


        }//End param'd Contact

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Associates()
        {
            return View();
        }

        public IActionResult Portfolio()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
