using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTGAHelper.Lib.Scraping.DeckSources.IoC;

namespace MTGAHelper.Lib.Scraping.DeckSources.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        static protected IServiceProvider provider;

        [AssemblyInitialize]
        public static void Setup(TestContext testContext)
        {
            provider = new ServiceCollection()
                .RegisterServicesScrapingDeckSources()
                .BuildServiceProvider();
        }
    }
}
