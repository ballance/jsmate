namespace JsMate.Api
{
    using System;
    using Nancy.Hosting.Self;

    class Program
    {
        static void Main(string[] args)
        {
            var uri =
                new Uri("http://localhost:9997");

            using (var host = new NancyHost(uri))
            {
                host.Start();

                Console.WriteLine("jsMate API is running on " + uri);
                Console.WriteLine("Press [Enter] to close the host.");
                Console.ReadLine();
            }
        }
    }
}
