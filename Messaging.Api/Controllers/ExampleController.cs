using System.Net;
using System.Net.Http;
using Messages.Commands;
using System.Web.Http;

namespace Messaging.Api
{
    public class ExampleController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            WebApiApplication.Bus.Send<IUpdateItemCommand>(x =>
            {
                x.ItemId = "fakeId";
                x.NewValue = 5;
            });

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

}
