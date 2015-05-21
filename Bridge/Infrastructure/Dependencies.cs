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
using Shared.DecisionTrees.DataStructure;
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

            #region Services
            Builder
                .RegisterType<YahooService>()
                .As<IYahooService>();

            Builder
                .RegisterType<ForexService>()
                .As<IForexService>();

            Builder
                .RegisterType<StatisticsService>()
                .As<IStatisticsService>();

            Builder
                .RegisterType<HistogramService>()
                .As<IHistogramService>();

            Builder
                .RegisterType<ForexMarketService>()
                .As<IForexMarketService>();

            Builder
                .RegisterType<ForexTradingAgentService>()
                .As<IForexTradingAgentService>();

            Builder
                .RegisterType<ForexTradingService>()
                .As<IForexTradingService>();
            #endregion

            #region Repositories
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
                .RegisterType<CsvDataRepository<StatisticsRecord>>()
                .As<ICsvDataRepository<StatisticsRecord>>();

            Builder
                .RegisterType<CsvDataRepository<ForexTreeData>>()
                .As<ICsvDataRepository<ForexTreeData>>();

            Builder
                .RegisterType<StatisticsResultsRepository>()
                .As<IStatisticsResultsRepository>();

            Builder
                .RegisterType<TradingResultsRepository>()
                .As<ITradingResultsRepository>();

            Builder
                .RegisterType<ForexMarketPathRepository>()
                .As<IForexMarketPathRepository>();

            Builder
                .RegisterType<DecisionTreesRepository>()
                .As<IDecisionTreesRepository>();

            Builder
                .RegisterType<HistogramResultsRepository>()
                .As<IHistogramResultsRepository>();

            Builder
                .RegisterType<ForexCsvRepository>()
                .As<IForexCsvRepository>();
            #endregion

            #region Decision Trees
            Builder
                .RegisterType<DecisionTreeReader>()
                .As<IDecisionTreeReader>();

            Builder
                .RegisterType<DecisionTree<ForexTreeData>>()
                .As<IDecisionTree<ForexTreeData>>();

            Builder
                .RegisterType<RuleBuilder>()
                .As<IRuleBuilder>();

            Builder
                .RegisterType<Classifier<ForexTreeData>>()
                .As<IClassifier<ForexTreeData>>();
            #endregion

        }

    }
}
