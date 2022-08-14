using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Extensions;

public static class SwaggerDocumentationConfig
{
    public static void Config(SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo {Title = "ComicShelf", Version = "v1"});

        var filePath = Path.Combine(AppContext.BaseDirectory, "Api.xml");
        swaggerGenOptions.IncludeXmlComments(filePath);

        swaggerGenOptions.AddSecurityDefinition("Bearer", securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
    }
}