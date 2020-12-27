using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Util.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Applicaion-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
