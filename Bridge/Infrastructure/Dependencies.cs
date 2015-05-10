#region Usings
using Autofac;
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Implementation.DLL.RepositoryBase;
#endregion

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
                .RegisterType<TreeDataRepository<YahooTreeData>>()
                .As<ITreeDataRepository<YahooTreeData>>();

            Builder
                .RegisterType<CsvDataRepository<YahooRecord>>()
                .As<ICsvDataRepository<YahooRecord>>();
        }

    }
}
