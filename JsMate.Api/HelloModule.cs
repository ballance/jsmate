using Nancy;

namespace JsMate.Api
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/hello/"] = _ => "hello there";
        }
    }
}
