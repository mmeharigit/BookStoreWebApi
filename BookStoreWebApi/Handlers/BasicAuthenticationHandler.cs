using BookStoreWebApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BookStoreWebApi.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly BookStoresDBContext _context;
        public BasicAuthenticationHandler(BookStoresDBContext context,IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock):base(options,logger, encoder, clock)
        {
            _context = context;
        }
#pragma// warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
#pragma //warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            return  AuthenticateResult.Fail("Authorization Header was not found");

            try
            {
            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
            string[] credential = Encoding.UTF8.GetString(bytes).Split(":");
            string emailAddress = credential[0];
            string password = credential[1];

            User user = _context.Users.Where(u => u.EmailAddress == emailAddress && u.Password == password).FirstOrDefault();

                if(user==null)
                {
                    return AuthenticateResult.Fail("Invalid Email or Password");
                }
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.EmailAddress) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var princiapal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(princiapal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception)
            {

                return AuthenticateResult.Fail("Error Has Occured");
            }
           

            //reading the Header Parameter value
            return AuthenticateResult.Fail("");
        }
    }
}
