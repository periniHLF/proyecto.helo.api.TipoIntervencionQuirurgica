using System.Text;
using System.Reflection;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using helo.lib.helpers;
using helo.lib.context.json;
using helo.api.TipoIntervencionQuirurgica;
using helo.api.TipoIntervencionQuirurgica.service;
using helo.api.TipoIntervencionQuirurgica.repository;
var builder = WebApplication.CreateBuilder(args);

var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

#region Declaracion de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetSection("JWTAutenticacion").GetValue<string>("Issuer"),
                    ValidAudience = builder.Configuration.GetSection("JWTAutenticacion").GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTAutenticacion").GetValue<string>("SecretKey")))
                };
            });
#endregion

#region Declaracion de Autommapper
var mapperConfig = new MapperConfiguration(m =>
    {
        m.AddProfile(new MappingProfile());
    });
IMapper mapper = mapperConfig.CreateMapper();
#endregion

#region Agregamos los servicio de la Aplicacion
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<IConfiguracionHelper, ConfiguracionHelper>();
builder.Services.AddSingleton<IConexionOracleContextJson, ConexionOracleContextJson>();

builder.Services.AddTransient<ITipoIntervencionQuirurgicaService, TipoIntervencionQuirurgicaService>();

builder.Services.AddTransient<ITipoIntervencionQuirurgicaRepository, TipoIntervencionQuirurgicaRepository>();
#endregion
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region Declaracion de interfaz de Swagger
builder.Services.AddSwaggerGen(options =>
{
    var version = builder.Configuration.GetValue<string>("Version");
    var fecha = builder.Configuration.GetValue<string>("Fecha");

    //Titulo
    options.SwaggerDoc("v1", new OpenApiInfo
    {
       Title = "helo.api.parametricos",
        Version = version,
        Description = $"API paramétrica para TipoIntervencionQuirurgica \n- Version {version} - Fecha {fecha}"
    });

    // Agregar boton Autorize
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autorizacion via JWT [recuerda ingresarlo como => [Bearer _token_]",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
       Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
# region Activa los CORS
app.UseCors(policy => policy.AllowAnyHeader()
.AllowAnyMethod()
.SetIsOriginAllowed(origin => true)
.AllowCredentials());
# endregion
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
