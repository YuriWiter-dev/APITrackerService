using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITracker_Service.Application.Interfaces
{
    public interface IEmail
    {
        void SendEmail(string to, string subject, string body);
    }
}
