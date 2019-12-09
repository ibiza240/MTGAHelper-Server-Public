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
    public class AllRatingsFileBuilder
    {
        const string filePathAllCardsCached = "../../../../data/AllCardsCached2.json";
        ICollection<Card> allCards = JsonConvert.DeserializeObject<ICollection<Card>>(File.ReadAllText(filePathAllCardsCached));

        [TestMethod]
        public void BuildFileAllRatings()
        {
            var lsvRatings = new ChannelFireballLsvDraftRatingsScraper(allCards).Scrape();
            var draftSimRatings = new DraftSimRatingsScraper(allCards).Scrape();

            var allRatings = new Dictionary<string, DraftRatings>
            {
                { "ChannelFireball (LSV)", lsvRatings },
                { "DraftSim", draftSimRatings },
            };

            // Master file that must contain all ratings information
            var filepath = Path.GetFullPath("draftRatings.json");
            File.WriteAllText(filepath, JsonConvert.SerializeObject(allRatings));
        }
    }
}
