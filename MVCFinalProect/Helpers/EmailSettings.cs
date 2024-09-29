using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
namespace MVC.Helpers
{
	public class EmailSetting
	{
        private readonly IConfiguration _configuration;

        public EmailSetting(IConfiguration configuration) 
		{
            _configuration = configuration;
        }
		public void SendEmail(Email email)
		{
			using (var client = new SmtpClient(_configuration["Email:Host"] , int.Parse(_configuration["Email:Port"])))
			{
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential(_configuration["Email:Sender"], _configuration["Email:Password"]);

				var mail = new MailMessage(_configuration["Email:Sender"], email.Reciepent, email.Subject, email.Body);

				try
				{
					client.Send(mail);
				}
				catch (Exception ex)
				{
					// Handle any exceptions here
					Console.WriteLine(ex.ToString());
				}
			}
		}
	}
}