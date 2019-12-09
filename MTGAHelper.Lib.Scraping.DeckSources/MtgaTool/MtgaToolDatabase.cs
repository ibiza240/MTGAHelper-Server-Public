using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MTGAHelper.Lib.Scraping.DeckSources.MtgaTool
{
    public class MtgaToolDatabase
    {
        [JsonProperty("cards")]
        public IDictionary<int, MtgaToolCard> Cards { get; set; }

        [JsonProperty("sets")]
        public IDictionary<string, MtgaToolSet> Sets { get; set; }
    }

    public class MtgaToolSet
    {
        [JsonProperty("arenacode")]
        public string SetCode { get; set; }
    }

    public class MtgaToolCard
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("cid")]
        public string CardId { get; set; } // needs to be a string, for example `GP1` is used as a Card Id
    }
}
