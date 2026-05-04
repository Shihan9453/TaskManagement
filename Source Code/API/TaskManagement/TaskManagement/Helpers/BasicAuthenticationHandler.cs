using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TaskManagement.Models;

namespace TaskManagement.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly AppDbContext _appDbContext;
        private readonly AppConstants _appConstants;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, AppDbContext appDbContext, AppConstants appConstants) : base(options, logger, encoder, clock)
        {
            _appDbContext = appDbContext;
            _appConstants = appConstants;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderValue.Scheme != "Basic") return AuthenticateResult.Fail("Invalid Scheme");

                var bytes = Convert.FromBase64String(authHeaderValue.Parameter);
                var credentials = Encoding.UTF8.GetString(bytes).Split(':');

                var username = credentials[0];
                var password = credentials[1];

                if (string.IsNullOrEmpty(username) == true && string.IsNullOrEmpty(password) == true)
                {
                    return AuthenticateResult.Fail(_appConstants.ERROR_USERNAME_PASSWORD_REQUIRED);
                }

                var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    return AuthenticateResult.Fail(_appConstants.ERROR_USERNAME_INVALID);
                }
                else
                {
                    if (user.UserPassword != password)
                    {
                        return AuthenticateResult.Fail(_appConstants.ERROR_PASSWORD_INVALID);
                    }
                }

                var claims = new[] { new Claim(ClaimTypes.Name, user.Username), new Claim("UserId", user.Id.ToString()) };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }

    }
}
