using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace JsMate.Api
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            pipelines.AfterRequest.AddItemToEndOfPipeline(c =>
            {
                c.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:9995";
            });
        }
    }
}
