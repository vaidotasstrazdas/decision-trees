#region Usings
using Autofac;
using Bridge.Infrastructure;
#endregion

namespace StatisticsComparer
{
    class Program
    {
        static void Main()
        {
            Dependencies.Register();

            var builder = Dependencies.Builder;

            builder
                .RegisterType<Application>()
                .As<IApplication>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();
                app.Run();
            }
        }
    }
}
