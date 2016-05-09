namespace Nancy.Api
{
    using System;

    using Nancy.Hosting.Self;

    public static class Program
    {
        private static void Main()
        {
            const int Port = 1234;
            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = new UrlReservations { CreateAutomatically = true }
            };

            using (var host = new NancyHost(new Uri($"http://localhost:{Port}"), new DefaultNancyBootstrapper(), hostConfiguration))
            {
                host.Start();
                Console.WriteLine($"Started running on post {Port}");
                Console.ReadLine();
            }
        }
    }
}