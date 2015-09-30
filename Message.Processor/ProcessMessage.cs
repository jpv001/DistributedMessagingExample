using NServiceBus;
using Messages.Commands;

namespace Message.Processor
{
    public class ProcessMessage : IHandleMessages<IUpdateItemCommand>
    {
        public void Handle(IUpdateItemCommand message)
        {
            //throw new System.NotImplementedException();
        }
    }
}
