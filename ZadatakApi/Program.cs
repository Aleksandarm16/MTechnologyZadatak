

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var app = builder.Build();


app.UseAuthorization();
app.UseRouting();

app.MapControllers();

app.Run();


// making the auto-generatet Program accessible programmatically for testing
public partial class Program { }
