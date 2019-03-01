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

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register RestClient for API
            builder.Register(x =>
                    new RestClient($"https://maps.googleapis.com/maps/api/place"))
                .Keyed<IRestClient>("Google");

            var placesKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GooglePlacesApiKey"];

            // Register API 
            builder.Register(x => new PlacesApiClient(
                x.ResolveKeyed<IRestClient>("Google"), placesKey
            )).As<IPlacesApiClient>();

            // Register PickUpContext
            builder.Register(context => new PickUpContext())
                .AsSelf()
                .InstancePerLifetimeScope();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
