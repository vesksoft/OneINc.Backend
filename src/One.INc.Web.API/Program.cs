using One.INc.Web.API;
var builder = WebApplication.CreateBuilder(args);

builder.RegisterDependencies();
builder.InitAndRun();

/// <summary>
/// new instance of Program. This declaration of the class is required for the integration testing server.
/// </summary>
public partial class Program
{
}