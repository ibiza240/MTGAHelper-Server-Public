using MTGAHelper.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MTGAHelper.Lib.Scraping.DraftHelper.DraftSim
{
    public partial class DraftSimRatingsScraper
    {
        string[] sets = new string[] { "ELD", "M20", "WAR", "RNA", "GRN" };
        string[] colors = new string[] { "W", "U", "B", "R", "G" };

        Dictionary<string, ICollection<string>> jsFilesPerSet = new Dictionary<string, ICollection<string>>
        {
            { "ELD", new [] { "ELD"} },
            { "M20", new [] { "M20", "M20_land" } },
            { "WAR", new [] { "WAR", "WAR_planeswalker" } },
            { "RNA", new [] { "RNA", /*"RNA_Orzhov", "RNA_Rakdos", "RNA_Azorius", "RNA_Gruul", "RNA_Simic",*/ "RNA_land", } },
            { "GRN", new [] { "GRN", /*"GRN_Boros", "GRN_Dimir", "GRN_Golgari", "GRN_Izzet", "GRN_Selesnya",*/ "GRN_land", } },
        };

        public class UrlToScrapeModel
        {
            public const string UrlTemplate = "https://draftsim.com/generated/{0}.js";

            public string UrlPartSet { get; set; }


            public UrlToScrapeModel(string urlPart)
            {
                UrlPartSet = urlPart;
            }
        }

        ICollection<Card> allCards;

        public DraftSimRatingsScraper(ICollection<Card> allCards)
        {
            this.allCards = allCards;
        }

        public DraftRatings Scrape(string setFilter = "")
        {
            var allCardsNames = allCards.Select(i => i.name).ToArray();
            var ret = new DraftRatings();
            var draftSimCardList = new List<DraftSimCard>();

            foreach (var set in sets.Where(i => setFilter == "" || i == setFilter))
            {
                var draftRatings = new List<DraftRating>();

                foreach (var fileTemplate in jsFilesPerSet[set])
                {
                    var urlFormatted = string.Format(UrlToScrapeModel.UrlTemplate, fileTemplate);
                    HttpClient client = new HttpClient();
                    var response = client.GetAsync(urlFormatted).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = response.Content.ReadAsStringAsync().Result;
                        jsonData = jsonData.Trim();
                        try
                        {
                            jsonData = jsonData.Substring(jsonData.IndexOf("["));  //this comes through as a javascript assignment.  remove anything prior to the start of the json array
                        }
                        catch
                        {
                            System.Diagnostics.Debugger.Break();
                        }

                        if (jsonData.EndsWith(";"))  //get rid of the ending javascript ; also
                        {
                            jsonData = jsonData.Remove(jsonData.Length - 1);
                        }
                        draftSimCardList = JsonConvert.DeserializeObject<List<DraftSimCard>>(jsonData);
                        foreach (var card in draftSimCardList)
                        {
                            card.name = card.name.Replace("_", " ");
                            if (allCardsNames.Contains(card.name) == false)
                            {
                                Console.WriteLine($"ACK.  {card.name} not found in list");
                                continue;
                            }
                            draftRatings.Add(new DraftRating { CardName = card.name, Rating = card.myrating, Description = "" });
                        }
                    }
                }

                var setinfo = new DraftRatingScraperResultForSet { Ratings = draftRatings };

                for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
                {
                    var top5Colors = draftSimCardList
                        .Where(i => i.colorsort == colorIndex && i.rarity.ToLower() == "c")
                        .OrderByDescending(i => i.myrating)
                        .Take(5)
                        .Select((i, idx) => new DraftRatingTopCard(idx + 1, i.name))
                        .ToArray();

                    setinfo.TopCommonCardsByColor.Add(colors[colorIndex], top5Colors);
                }

                ret.RatingsBySet.Add(set, setinfo);
            }

            return ret;

        }
    }

}
