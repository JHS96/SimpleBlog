using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SimpleBlogAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"Simple Blog API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer Scheme. \r\n\r\n Enter: 'Bearer' " +
                                "[space] and then your token in the text input below. " +
                                "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            options.IncludeXmlComments(xmlCommentsFullPath);
        }
    }
}
