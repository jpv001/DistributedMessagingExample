using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus.Hosting.Azure;

namespace Messaging.Endpoint
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly NServiceBusRoleEntrypoint _nsb = new NServiceBusRoleEntrypoint();

        public override bool OnStart()
        {
            _nsb.Start();
            return base.OnStart();
        }

        public override void OnStop()
        {
            _nsb.Stop();
            base.OnStop();
        }
    }
}
