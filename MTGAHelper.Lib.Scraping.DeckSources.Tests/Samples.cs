using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using MTGAHelper.Entity.DeckScraper;
using MTGAHelper.Lib.Scraping.DeckSources.MtgGoldfish;

namespace MTGAHelper.Lib.Scraping.DeckSources.Tests
{
    [TestClass]
    public class Samples : TestBase
    {
        [TestMethod]
        public void SampleMtgTop8()
        {
            var scraper = provider.GetService<MtgTop8Scraper>();
            var decks = scraper.GetDeckList();
            foreach (var deckInput in decks)
            {
                var textDeck = scraper.GetDeck(deckInput);
                Debug.WriteLine($"Deck found at {deckInput.UrlViewDeck}:{Environment.NewLine}{textDeck}");
            }
        }

        [TestMethod]
        public void SampleMtgaToolBo1()
        {
            var scraper = provider.GetService<MtgaToolScraper>().Init(MtgaToolFormatEnum.Bo1);
            var decks = scraper.GetDeckList();
            foreach (var deckInput in decks)
            {
                var textDeck = scraper.GetDeck(deckInput);
                Debug.WriteLine($"Deck found at {deckInput.UrlViewDeck}:{Environment.NewLine}{textDeck}");
                Assert.AreEqual("https://mtgatool.com/metagame/BO1", deckInput.UrlDeckList);
            }
        }

        [TestMethod]
        public void SampleMtgaToolBo3()
        {
            var scraper = provider.GetService<MtgaToolScraper>().Init(MtgaToolFormatEnum.Bo3);
            var decks = scraper.GetDeckList();
            foreach (var deckInput in decks)
            {
                var textDeck = scraper.GetDeck(deckInput);
                Debug.WriteLine($"Deck found at {deckInput.UrlViewDeck}:{Environment.NewLine}{textDeck}");
                Assert.AreEqual("https://mtgatool.com/metagame/BO3", deckInput.UrlDeckList);
            }
        }

        [TestMethod]
        public void SampleMtgGoldfishTournaments()
        {
            var scraper = provider.GetService<MtgGoldfishTournamentsScraper>();
            var decks = scraper.GetDeckList();
            foreach (var deckInput in decks)
            {
                var textDeck = scraper.GetDeck(deckInput);
                Debug.WriteLine($"Deck found at {deckInput.UrlViewDeck}:{Environment.NewLine}{textDeck}");
            }
        }
    }
}
