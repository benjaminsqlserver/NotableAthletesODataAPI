using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NotableAthletes.Data;
using NotableAthletes.Models.ConData;

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Country>("Countries");
    builder.EntitySet<Athlete>("Athletes");
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ConDataContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConDataConnection"))
);
builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("v1", GetEdmModel()).Filter().Select().Expand());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Notable Atheletes API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notable Athletes API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
