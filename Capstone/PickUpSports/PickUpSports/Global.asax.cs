using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using PickUpSports.DAL;
using PickUpSports.DAL.Repositories;
using PickUpSports.GoogleApi;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Services;
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

            // Register services
            builder.RegisterType<VenueService>().As<IVenueService>();
            builder.RegisterType<ContactService>().As<IContactService>();

            // Register repositories
            builder.RegisterType<ContactRepository>().As<IContactRepository>();
            builder.RegisterType<TimePreferenceRepository>().As<ITimePreferenceRepository>();
            builder.RegisterType<SportPreferenceRepository>().As<ISportPreferenceRepository>();
            builder.RegisterType<ReviewRepository>().As<IReviewRepository>();
            builder.RegisterType<SportRepository>().As<ISportRepository>();
            builder.RegisterType<PickUpGameRepository>().As<IPickUpGameRepository>();

            var container = builder.Build();

            // Resolve all registrations above 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
