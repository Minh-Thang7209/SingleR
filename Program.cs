using SingleR.Hubs;
namespace SingleR;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapHub<ChatHub>("/chatHub");

        app.Run();
    }
}
