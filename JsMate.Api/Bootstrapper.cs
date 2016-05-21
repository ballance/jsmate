using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace JsMate.Api
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<JsonSerializer, CustomJsonSerializer>();
        }
    }
}