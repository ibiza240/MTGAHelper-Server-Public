using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MTGAHelper.Entity;
using MTGAHelper.Lib.Config;
using MTGAHelper.Lib.DeckScraper;

namespace MTGAHelper.Lib.Scraping.DeckSources
{
    public class MtgTop8Scraper : IDeckScraper
    {
        public string SiteUrl => "https://www.mtgtop8.com/";

        /// <summary>
        /// This is used to scrape the page that contains the list of decks wanted
        /// </summary>
        /// <returns>Each item in the list returned contains the necessary information to scrape this individual deck</returns>
        public ICollection<DeckScraperDeckInputs> GetDeckList()
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load($"{SiteUrl}format?f=ST");

            // Look up for the "The decks to beat" on the main page because it changes monthly (top right corner visually)
            var linkDecksToBeat = doc.DocumentNode.SelectNodes("//a").FirstOrDefault(i => i.InnerText.ToLower().Contains("decks to beat"));

            if (linkDecksToBeat == null)
                return new DeckScraperDeckInputs[0];

            // Load the "Decks to beat url"
            var urlDeckList = $"{SiteUrl}{linkDecksToBeat.Attributes["href"].Value}";
            doc = hw.Load(urlDeckList);

            // Scrape the HTML parts
            var divDecks = doc.DocumentNode.SelectNodes("//table//div").FirstOrDefault(i => i.InnerText.ToLower().Contains("tier 1"));

            if (divDecks == null)
                return new DeckScraperDeckInputs[0];

            var links = divDecks.SelectNodes(".//a").Where(i => i.Attributes["href"].Value.Contains("d=")).ToArray();
            var regexDeckId = new Regex(@"d=(\d+)");

            var strDate = divDecks.SelectSingleNode(".//td").InnerText.Replace("Standard", "").Trim();
            var date = DateTime.ParseExact(strDate, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None);

            // Plug in the data found in an input for each deck found
            var inputs = links
                .Select(i =>
                {
                    var urlDeck = i.Attributes["href"].Value;
                    var deckId = regexDeckId.Match(urlDeck).Groups[1].Value;

                    return new DeckScraperDeckInputs(i.InnerText)
                    {
                        UrlViewDeck = $"{SiteUrl}event{urlDeck}",
                        UrlDownloadDeck = $"{SiteUrl}mtgarena?d={deckId}",
                        DateCreated = date,
                        UrlDeckList = urlDeckList,
                    };
                })
                .ToArray();

            return inputs;
        }

        /// <summary>
        /// This is used to scrape a single deck by calling the function on the webpage that copies the deck to clipboard
        /// </summary>
        /// <param name="input">The necessary properties to scrape the deck properly</param>
        /// <returns>The text deck that can be imported in MTGA</returns>
        public string GetDeck(DeckScraperDeckInputs input)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(input.UrlDownloadDeck);

            var regexTextDeck = new Regex("innerHTML=\"(.*?)\"");

            var textDeck = regexTextDeck.Match(doc.DocumentNode.InnerHtml).Groups[1].Value.Replace(@"\n", Environment.NewLine);
            return textDeck;
        }
    }
}
