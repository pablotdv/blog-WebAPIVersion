using System.Web.Http;
using Swashbuckle.Application;
using Owin;
using Microsoft.Web.Http.Routing;
using System.Web.Http.Routing;
using System.Web.Http.Description;
using WebAPIVersion.Infraestrutura;

namespace WebAPIVersion
{
    public partial class Startup 
    {
        public static void ConfigureSwagger(IAppBuilder builder)
        {
            // we only need to change the default constraint resolver for services that want urls with versioning like: ~/v{version}/{controller}
            var constraintResolver = new DefaultInlineConstraintResolver() { ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) } };
            var configuration = new HttpConfiguration();
            var httpServer = new HttpServer(configuration);

            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            configuration.AddApiVersioning(o => o.ReportApiVersions = true);
            configuration.MapHttpAttributeRoutes(constraintResolver);

            // add the versioned IApiExplorer and capture the strongly-typed implementation (e.g. VersionedApiExplorer vs IApiExplorer)
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            var apiExplorer = configuration.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

            configuration.EnableSwagger(
                            "{apiVersion}/swagger",
                            swagger =>
                            {
                                // build a swagger document and endpoint for each discovered API version
                                swagger.MultipleApiVersions(
                                    (apiDescription, version) => apiDescription.GetGroupName() == version,
                                    info =>
                                    {
                                        foreach (var group in apiExplorer.ApiDescriptions)
                                        {
                                            var description = "A sample application with Swagger, Swashbuckle, and API versioning.";

                                            if (group.IsDeprecated)
                                            {
                                                description += " This API version has been deprecated.";
                                            }

                                            info.Version(group.Name, $"Sample API {group.ApiVersion}")
                                                .Contact(c => c.Name("Bill Mei").Email("bill.mei@somewhere.com"))
                                                .Description(description)
                                                .License(l => l.Name("MIT").Url("https://opensource.org/licenses/MIT"))
                                                .TermsOfService("Shareware");
                                        }
                                    });

                                // add a custom operation filter which sets default values
                                swagger.OperationFilter<SwaggerDefaultValues>();

                                // integrate xml comments
                                // swagger.IncludeXmlComments(XmlCommentsFilePath);
                            })
                         .EnableSwaggerUi(swagger => swagger.EnableDiscoveryUrlSelector());

            builder.UseWebApi(httpServer);
        }
    }
}
