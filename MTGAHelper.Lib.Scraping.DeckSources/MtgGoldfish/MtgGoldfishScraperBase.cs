using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using MTGAHelper.Entity;
using MTGAHelper.Lib.Config;
using MTGAHelper.Lib.Exceptions;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace MTGAHelper.Lib.Scraping.DeckSources.MtgGoldfish
{
    public abstract class MtgGoldfishScraperBase
    {
        public class MtgGoldfishThrottlingException : Exception
        {
        }

        //protected abstract string DeckUrl(string urlPart);
        public string SiteUrl => "https://www.mtggoldfish.com";
        
        protected HtmlDocument LoadMtgGoldfishUrl(string url)
        {
            try
            {
                HtmlWeb hw = new HtmlWeb();
                HtmlDocument doc = hw.Load(url);

                // MTGGoldfish throttling
                if (doc.DocumentNode.InnerHtml.Trim() == "Throttled")
                    throw new MtgGoldfishThrottlingException();

                return doc;
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debugger.Break();
                Log.Error("Problem trying to download deck at: {url}", url);
                throw;
            }
        }

        protected string GetTextAreaContent(string urlDownloadDeck)
        {
            //try
            //{
            var doc = LoadMtgGoldfishUrl(urlDownloadDeck);

            string deckText = "";
            try
            {
                deckText = WebUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("//textarea").InnerText);
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debugger.Break();
                throw;
            }
            // Copy the textarea content
            return deckText;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex, urlDownloadDeck);
            //    System.Diagnostics.Debugger.Break();
            //    return "";
            //}
        }

        public DeckScraperDeckInputs DownloadDeckFromDeckView(string urlViewDeck)
        {
            var doc = LoadMtgGoldfishUrl(urlViewDeck);

            try
            {
                var name = doc.DocumentNode.SelectSingleNode("//h2").InnerText.Split(new[] { "Suggest" }, StringSplitOptions.None)[0].Trim();
                if (name == "Sample Deck" || name == "Description")
                    name = doc.DocumentNode.SelectSingleNode("//h1[@class='deck-view-title']").InnerText
                        .Split(new[] { "Suggest" }, StringSplitOptions.None)[0]
                        .Trim()
                        .Split(new[] { '\n' }, StringSplitOptions.None)[0]
                        .Trim();

                // Get date created
                var str = doc.DocumentNode.SelectSingleNode("//div[@class='deck-view-description']").InnerText.Trim();
                var regexDate = new Regex(@"20\d\d-\d\d-\d\d", RegexOptions.Multiline);
                var strDate = regexDate.Match(str).Groups[1].Value;
                if (string.IsNullOrWhiteSpace(strDate))
                {
                    strDate = str.Split('\n').Last().Trim();
                }

                var dateCreated = ParseDateCreated(strDate, name, urlViewDeck);

                var linksContainingArenaDownload = doc.DocumentNode.SelectNodes("//div[@class='deck-view-tools']//a");
                var urlDownloadDeck = SiteUrl + linksContainingArenaDownload
                    .Single(i => i.Attributes["href"].Value.StartsWith("/deck/arena_download"))
                    .Attributes["href"].Value;

                var inputDeck = new DeckScraperDeckInputs(name, dateCreated) { UrlViewDeck = urlViewDeck, UrlDownloadDeck = urlDownloadDeck };
                return inputDeck;
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debugger.Break();
                throw;
            }
        }

        protected DateTime ParseDateCreated(string strDate, string deckName, string url = null)
        {
            if (DateTime.TryParse(strDate, out DateTime dt))
                return dt;
            else
                return DateTime.MinValue;
        }
    }
}
