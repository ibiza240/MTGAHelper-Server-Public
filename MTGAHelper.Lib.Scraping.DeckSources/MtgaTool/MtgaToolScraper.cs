using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using MTGAHelper.Entity.DeckScraper;
using MTGAHelper.Lib.DeckScraper;
using MTGAHelper.Lib.Scraping.DeckSources.MtgaTool;
using Newtonsoft.Json;

namespace MTGAHelper.Lib.Scraping.DeckSources
{
    public class MtgaToolScraper : IDeckScraper
    {
        public string SiteUrl { get; } = "https://mtgatool.com/";

        Regex databaseRegex => new Regex("let database = (\\{.*\\});");
        Regex responseRegex => new Regex("let response = (\\{.*\\});");

        MtgaToolFormatEnum format;

        public MtgaToolScraper Init(MtgaToolFormatEnum format)
        {
            this.format = format;
            return this;
        }

        /// <summary>
        /// This is used to scrape the page that contains the list of decks wanted
        /// </summary>
        /// <returns>Each item in the list returned contains the necessary information to scrape this individual deck</returns>
        public ICollection<DeckScraperDeckInputs> GetDeckList()
        {
            var urlDeckList = $"{SiteUrl}metagame/{format.ToString().ToUpper()}";
            var hw = new HtmlWeb();
            var doc = hw.Load(urlDeckList);

            // Get the 'response' variable from the JavaScript, it's a JSON string.
            var scriptNodes = doc.DocumentNode.SelectNodes("/html/script");
            var innerHtml = scriptNodes[0].InnerHtml;

            var jsonDb = databaseRegex.Match(innerHtml).Groups[1].Value;
            var jsonResp = responseRegex.Match(innerHtml).Groups[1].Value;

            var response = JsonConvert.DeserializeObject<MtgaToolResponse>(jsonResp);
            var db = JsonConvert.DeserializeObject<MtgaToolDatabase>(jsonDb);

            var dayId = response.Id.Split('.')[0];

            return response.Archetypes
                .Where(a => a.Name != "Unknown")
                .Select(a => new DeckScraperDeckInputs(a.Name, response.Date)
                {
                    DeckText = getDeckText(a.BestDeck, db),
                    UrlDeckList = urlDeckList,
                    UrlViewDeck = $"{SiteUrl}metagame/{format.ToString().ToUpper()}/{dayId}/{Uri.EscapeUriString(a.Name)}"
                })
                .ToArray();
        }

        /// <summary>
        /// This is used to scrape a single deck by calling the function on the webpage that copies the deck to clipboard
        /// </summary>
        /// <param name="input">The necessary properties to scrape the deck properly</param>
        /// <returns>The text deck that can be imported in MTGA</returns>
        public string GetDeck(DeckScraperDeckInputs input)
        {
            return input.DeckText;
        }

        string getDeckText(MtgaToolDeck deck, MtgaToolDatabase db)
        {
            var mainDeckTxt = string.Join(Environment.NewLine, deck.MainDeck.Select(c => getCardText(c, db)));
            var sideBoardTxt = string.Join(Environment.NewLine, deck.SideBoard.Select(c => getCardText(c, db)));

            return $"{mainDeckTxt}{Environment.NewLine}{Environment.NewLine}{sideBoardTxt}";
        }

        string getCardText(MtgaToolBoard cardInDeck, MtgaToolDatabase db)
        {
            var card = db.Cards[cardInDeck.Id];
            var setCode = db.Sets[card.Set].SetCode;
            return $"{cardInDeck.Qty} {card.Name} ({setCode}) {card.CardId}";
        }
    }
}
