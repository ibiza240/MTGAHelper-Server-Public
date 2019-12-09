using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTGAHelper.Entity;
using MTGAHelper.Lib.Scraping.DraftRatingsScraper;
using MTGAHelper.Lib.Scraping.DraftHelper.DraftSim;
using Newtonsoft.Json;
using System.Linq;

namespace MTGAHelper.Lib.Scraping.DraftHelper.Tests
{
    [TestClass]
    public class Samples
    {
        const string filePathAllCardsCached = "../../../../data/AllCardsCached2.json";
        ICollection<Card> allCards = JsonConvert.DeserializeObject<ICollection<Card>>(File.ReadAllText(filePathAllCardsCached));

        [TestMethod]
        public void SampleChannelFireball()
        {
            var scraper = new ChannelFireballLsvDraftRatingsScraper(allCards);

            var results = scraper.Scrape("ELD");
        }

        [TestMethod]
        public void SampleDraftSim()
        {
            // https://draftsim.com/
            var scraper = new DraftSimRatingsScraper(allCards);
            var results = scraper.Scrape();
            //var test = results.RatingsBySet["GRN"].Ratings.GroupBy(i => i.CardName).Where(i => i.Count() > 1).ToArray();
            //var test2 = test.Where(i => i.Key.StartsWith("Crackling"));
        }

        [TestMethod]
        public void SampleFrankKarsten()
        {
            // https://docs.google.com/spreadsheets/d/e/2PACX-1vSX3Jjurk3DJmYrlufl_U8my9A0iiXZLwIm4O7Li1e2REwcZJSR5DXAQhgamCi60CsYpmBWNUsYCbjJ/pubhtml?gid=0
        }

        [TestMethod]
        public void SampleDeathsie()
        {
            // https://docs.google.com/spreadsheets/u/1/d/1L8OiegAETD5k393PGegq7Ps5sgsRi-0Zqy4r3e4qULg/htmlview#
        }

        [TestMethod]
        public void SampleDraftaholicsAnonymous()
        {
            // http://www.draftaholicsanonymous.com/
        }

        [TestMethod]
        public void SampleCommunityReview()
        {
            // https://www.mtgcommunityreview.com/
        }

        [TestMethod]
        public void Sample17Lands()
        {
            // https://www.17lands.com/
        }
    }
}
