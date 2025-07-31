using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Data;

public class DbTestController : Controller
{
    private readonly ApplicationDbContext _context;
    public DbTestController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        try
        {
            var count = _context.Users.Count(); // or any simple query
            return Content("DB Connected! Users count: " + count);
        }
        catch (Exception ex)
        {
            return Content("DB Connection failed: " + ex.Message);
        }
    }
}