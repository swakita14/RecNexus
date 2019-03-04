using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using PickUpSports.Data.GoogleAPI;
using PickUpSports.Data.GoogleAPI.Interfaces;
using PickUpSports.DAL;
using RestSharp;

namespace PickUpSports
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Register all controllers in our MVC project 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register RestClient for API with base URL 
            builder.Register(x =>
                    new RestClient($"https://maps.googleapis.com/maps/api/place"))
                .Keyed<IRestClient>("Google");

            // Get Google API key from app settings
            var placesKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GooglePlacesApiKey"];

            // Register API client class with the RestClient and API key as a parameter
            builder.Register(x => new PlacesApiClient(
                x.ResolveKeyed<IRestClient>("Google"), placesKey
            )).As<IPlacesApiClient>();

            // Register PickUpContext for database
            builder.Register(context => new PickUpContext())
                .AsSelf()
                .InstancePerRequest();

            var container = builder.Build();

            // Resolve all registrations above 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
