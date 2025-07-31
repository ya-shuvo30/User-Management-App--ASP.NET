using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System.Linq;
using UserManagementApp.Data;

public class BlockUserMiddleware
{
    private readonly RequestDelegate _next;

    public BlockUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        var path = context.Request.Path.Value?.ToLower();

        // Allow login and register requests to pass through
        if (path != null && (path.Contains("/account/login") || path.Contains("/account/register")))
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var email = context.User.Identity.Name;
            var user = dbContext.Users.FirstOrDefault(u => u.Email == email);

            // Make sure your User model has IsBlocked and IsDeleted properties
            if (user != null && (user.IsBlocked || user.IsDeleted))
            {
                await context.SignOutAsync();
                context.Response.Redirect("/Account/Login?blocked=true");
                return;
            }
        }

        await _next(context);
    }
}