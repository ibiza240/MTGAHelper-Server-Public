using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MTGAHelper.Lib.Scraping.DeckSources.MtgaTool
{
    public class MtgaToolResponse
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("meta")]
        public ICollection<MtgaToolArchetype> Archetypes { get; set; }
    }

    public class MtgaToolArchetype
    {
        [JsonProperty("total")]
        public int TotalDecks { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("best_deck")]
        public MtgaToolDeck BestDeck { get; set; }
    }

    public class MtgaToolDeck
    {
        [JsonProperty("mainDeck")]
        public ICollection<MtgaToolBoard> MainDeck { get; set; }

        [JsonProperty("sideboard")]
        public ICollection<MtgaToolBoard> SideBoard { get; set; }
    }

    public class MtgaToolBoard
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("quantity")]
        public int Qty { get; set; }
    }
}
