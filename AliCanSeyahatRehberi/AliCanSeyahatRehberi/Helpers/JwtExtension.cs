using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliCanSeyahatRehberi.Helpers
{
    public  static class JwtExtensions
    {
        public static void AddAplicationError(this HttpResponse response,string message)
        {
            //aşağıda " " çinde yazdıklarım uydurma şeyler değil JWt için Gerekli//
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Allow-Origin","*");
            response.Headers.Add("Access-Control-Expose","Application-Error" );

        }
    }
}
