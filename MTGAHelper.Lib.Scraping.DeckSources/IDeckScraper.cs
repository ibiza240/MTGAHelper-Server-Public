using System;
using System.Collections.Generic;
using System.Text;
using MTGAHelper.Lib.DeckScraper;

namespace MTGAHelper.Lib.Scraping.DeckSources
{
    public interface IDeckScraper
    {
        ICollection<DeckScraperDeckInputs> GetDeckList();
        string GetDeck(DeckScraperDeckInputs input);
    }
}
