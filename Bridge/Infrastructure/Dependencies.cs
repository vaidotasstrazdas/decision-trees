#region Usings
using Autofac;
using Bridge.IBLL.Data;
using Bridge.IBLL.Interfaces;
using Bridge.IDLL.Data;
using Bridge.IDLL.Interfaces;
using Implementation.BLL;
using Implementation.DLL;
using Implementation.DLL.RepositoryBase;
using Shared.DecisionTrees;
using Shared.DecisionTrees.Interfaces;

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
                .RegisterType<DecisionTreeReader>()
                .As<IDecisionTreeReader>();

            Builder
                .RegisterType<YahooService>()
                .As<IYahooService>();

            Builder
                .RegisterType<ForexService>()
                .As<IForexService>();

            Builder
                .RegisterType<TreeDataRepository<YahooTreeData>>()
                .As<ITreeDataRepository<YahooTreeData>>();

            Builder
                .RegisterType<TreeDataRepository<ForexTreeData>>()
                .As<ITreeDataRepository<ForexTreeData>>();

            Builder
                .RegisterType<CsvDataRepository<YahooRecord>>()
                .As<ICsvDataRepository<YahooRecord>>();

            Builder
                .RegisterType<ForexCsvRepository>()
                .As<IForexCsvRepository>();
        }

    }
}
