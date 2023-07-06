using APITracker_Service.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APITracker_Service.Helper;

namespace APITracker_Service.Application.Services
{
    public class EmailService : IEmail
    {
        public void SendEmail(string to, string subject, string body)
        {
            var address = new EmailHelper("outlook.office365.com", "yuri.lightbase@sebraemg.com.br", ENV.PASSWORD);
            address.SendEmail(new List<string> { to }, subject, body, new());
        }
    }
}
