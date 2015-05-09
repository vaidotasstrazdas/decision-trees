using Autofac;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Implementation.DLL;
using Implementation.DLL.RepositoryBase;

namespace Bridge.Infrastructure
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
