using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyWineCellar.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

public class BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger, UrlEncoder encoder, WineCellarDbContext db)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var header))
            return AuthenticateResult.Fail("Missing Authorization header");

        var encoded = header.ToString().Replace("Basic ", "");
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        var parts = decoded.Split(':', 2);

        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == parts[0] && u.Password == parts[1]);
        if (user is null)
            return AuthenticateResult.Fail("Invalid credentials");

        var claims = new[] { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name)), Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}