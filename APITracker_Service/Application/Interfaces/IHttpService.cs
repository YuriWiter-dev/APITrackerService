using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITracker_Service.Application.Interfaces
{
    public interface IHttpService
    {
        Task<HttpStatusCode> Check(string URL);
    }
}
