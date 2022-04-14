using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleBlog;
using SimpleBlogAPI;
using SimpleBlogAPI.Mapper;
using SimpleBlogAPI.Repository;
using SimpleBlogAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<SimpleBlogDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));

builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddAutoMapper(typeof(SimpleBlogMappings));

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

//Add Authentication Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://simpleblog.eu.auth0.com/";
    options.Audience = "https://localhost:44355/";
});

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

//https://stackoverflow.com/questions/70386368/how-to-access-dbcontext-in-net-6-minimal-api-program-cs
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SimpleBlogDbContext>();
    dbContext.Database.EnsureCreated();
    // use context
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
//https://stackoverflow.com/questions/65879195/dotnet-5-0-swagger-is-not-loading-in-azure-app-service
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lotus.API.Integration v1"));

app.UseHttpsRedirection();

//Enable authentication middleware
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
