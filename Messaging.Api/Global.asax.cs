using System.Web.Http;
using System.Web.Mvc;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;

namespace Messaging.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static ISendOnlyBus _bus;

        public static ISendOnlyBus Bus
        {
            get { return _bus; }
        }

        protected void Application_Start()
        {

            var config = new BusConfiguration();

            config.Conventions()
                    .DefiningCommandsAs(t => t.Namespace != null && (t.Namespace.StartsWith("Commands") || t.Namespace.Contains("Commands")))
                    .DefiningEventsAs(t => t.Namespace != null && (t.Namespace.EndsWith("Events") || t.Namespace.EndsWith("Notifications")));

            LogManager.Use<DefaultFactory>().Directory("c:/temp");

            config.UseTransport<AzureServiceBusTransport>();
            //config.UsePersistence<AzureStoragePersistence>();

            config.PurgeOnStartup(true);
            config.EnableInstallers();

            _bus = NServiceBus.Bus.CreateSendOnly(config);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }

    // We don't need it, so instead of configuring it, we disable it
    public class DisableTimeoutManager : INeedInitialization
    {
        public void Customize(BusConfiguration config)
        {
            config.DisableFeature<TimeoutManager>();
            config.DisableFeature<SecondLevelRetries>();
            config.DisableFeature<Sagas>();
        }
    }
}
