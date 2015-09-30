using System;
using System.Diagnostics;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Azure;

namespace Messaging.Endpoint
{
    public class Endpoint : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UseTransport<AzureServiceBusTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            
            configuration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && (t.Namespace.StartsWith("Commands") || t.Namespace.Contains("Commands")))
                .DefiningEventsAs(t => t.Namespace != null && (t.Namespace.EndsWith("Events") || t.Namespace.EndsWith("Notifications")));

            // Configuring how NServicebus handles critical errors - pre version 6.2.2
            configuration.DefineCriticalErrorAction((message, exception) =>
            {
                var errorMessage = string.Format("We got a critical exception: '{0}'\r\n{1}", message, exception);
                var fatalMessage = string.Format("The following critical error was encountered by NServiceBus:\n{0}\nNServiceBus is shutting down.", errorMessage);
                Environment.FailFast(fatalMessage, exception);
            });
        }
    }

    public class StartupTasks : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            Trace.WriteLine(string.Format("The MessagingEndpoint is now started and ready to accept messages"));
        }

        public void Stop()
        {

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
