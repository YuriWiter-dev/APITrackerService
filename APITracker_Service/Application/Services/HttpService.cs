using APITracker_Service.Application.Interfaces;
using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http.Json;
using System.Net.Http;

namespace APITracker_Service.Application.Services
{
    public class HttpService :IHttpService
    {
        public async Task<HttpStatusCode> Check(string URL) { 
            var client = new HttpClient();
			try
			{
				var response = await client.GetAsync(URL);
				return response.StatusCode; 
			}
			catch (HttpRequestException)
			{

				return HttpStatusCode.NotFound;
			}
			finally {
				client.Dispose();
			}
        }
    }
}
