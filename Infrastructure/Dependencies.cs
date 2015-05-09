using Autofac;
using BLL;
using IBLL.Interfaces;
using DLL;
using IDLL.Data;
using IDLL.Interfaces;

namespace Infrastructure
{
    public static class Dependencies
    {

        public static ContainerBuilder Builder { get; set; }

        public static void Register()
        {
            Builder = new ContainerBuilder();

            Builder
                .RegisterType<YahooService>()
                .As<IYahooService>();

            Builder
                .RegisterType<CsvDataRepository<YahooRecord>>()
                .As<ICsvDataRepository<YahooRecord>>();
        }

    }
}
