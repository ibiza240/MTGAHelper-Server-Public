using MTGAHelper.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGAHelper.Lib.Scraping.DraftRatingsScraper
{
    public partial class ChannelFireballLsvDraftRatingsScraper
    {
        static readonly Dictionary<string, string> colors = new Dictionary<string, string> {
            {"W", "white" },
            {"U", "blue" },
            {"B", "black" },
            {"R", "red" },
            {"G", "green" },
        };

        Dictionary<string, UrlToScrapeModel> modelBySet = new Dictionary<string, UrlToScrapeModel>()
        {
            { "ELD", new UrlToScrapeModel("throne-of-eldraine", colors.And("gold", "artifacts-and-gold-cards").And("lands", "artifacts-and-lands")) },
            { "M20", new UrlToScrapeModel("core-set-2020", colors.And("", "gold-artifacts-and-lands")) },
            { "WAR", new UrlToScrapeModel("war-of-the-spark", colors.And("", "gold-artifacts-and-lands")) },
            { "RNA", new UrlToScrapeModel("ravnica-allegiance", colors.And(new Dictionary<string, string>
            {
                { "WU", "azorius" },
                { "RG", "gruul" },
                { "WB", "orzhov" },
                { "BR", "rakdos" },
                { "UG", "simic-and-colorless" },
            })) },
            { "GRN", new UrlToScrapeModel("guilds-of-ravnica", colors.And(new Dictionary<string, string>
            {
                { "WR", "boros" },
                { "UB", "dimir" },
                { "BG", "golgari" },
                { "UR", "izzet" },
                { "WG", "selesnya" },
                { "", "artifacts-lands-and-guild-ranking" },
            })) },
        };
        Dictionary<string, ICollection<DraftRating>> manualRatings = new Dictionary<string, ICollection<DraftRating>>
            {
                { "ELD", new DraftRating[] {
                }},
                { "M20", new [] {
                    new DraftRating
                    {
                        CardName = "Bloodfell Caves",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Blossoming Sands",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Dismal Backwater",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Jungle Hollow",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Wind-Scarred Crag",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Rugged Highlands",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Scoured Barrens",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Swiftwater Cliffs",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Thornwood Falls",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Tranquil Cove",
                        Rating = "3.0",
                        Description = "All of these lands are nice additions to any 2-color deck, and make splashing a third color much easier. I tend to take them a little higher than average playables, but under anything premium.",
                    },
                    new DraftRating
                    {
                        CardName = "Temple of Triumph",
                        Rating = "3.5",
                        Description = "Scry 1 is a real advantage, and I’d take these Temples aggressively. They are rare, so it doesn’t come up a ton, but treat them like real playables (and play them even when half on color).",
                    },
                    new DraftRating
                    {
                        CardName = "Temple of Mystery",
                        Rating = "3.5",
                        Description = "Scry 1 is a real advantage, and I’d take these Temples aggressively. They are rare, so it doesn’t come up a ton, but treat them like real playables (and play them even when half on color).",
                    },
                    new DraftRating
                    {
                        CardName = "Temple of Silence",
                        Rating = "3.5",
                        Description = "Scry 1 is a real advantage, and I’d take these Temples aggressively. They are rare, so it doesn’t come up a ton, but treat them like real playables (and play them even when half on color).",
                    },
                    new DraftRating
                    {
                        CardName = "Temple of Epiphany",
                        Rating = "3.5",
                        Description = "Scry 1 is a real advantage, and I’d take these Temples aggressively. They are rare, so it doesn’t come up a ton, but treat them like real playables (and play them even when half on color).",
                    },
                    new DraftRating
                    {
                        CardName = "Temple of Malady",
                        Rating = "3.5",
                        Description = "Scry 1 is a real advantage, and I’d take these Temples aggressively. They are rare, so it doesn’t come up a ton, but treat them like real playables (and play them even when half on color).",
                    }
                }},
                { "WAR", new DraftRating[] {
                }},
                { "RNA", new DraftRating[] {
                    new DraftRating
                    {
                        CardName = "Azorius Locket",
                        Rating = "2.0",
                        Description = "Lockets get a bad rap. In slower decks, they are often worth a slot, as they help cast expensive spells and can be cracked for a pair of cards later. The non-red ones are better, as the red decks are often lower to the ground, but all of them have their uses.",
                    },
                    new DraftRating
                    {
                        CardName = "Gruul Locket",
                        Rating = "2.0",
                        Description = "Lockets get a bad rap. In slower decks, they are often worth a slot, as they help cast expensive spells and can be cracked for a pair of cards later. The non-red ones are better, as the red decks are often lower to the ground, but all of them have their uses.",
                    },
                    new DraftRating
                    {
                        CardName = "Orzhov Locket",
                        Rating = "2.0",
                        Description = "Lockets get a bad rap. In slower decks, they are often worth a slot, as they help cast expensive spells and can be cracked for a pair of cards later. The non-red ones are better, as the red decks are often lower to the ground, but all of them have their uses.",
                    },
                    new DraftRating
                    {
                        CardName = "Rakdos Locket",
                        Rating = "2.0",
                        Description = "Lockets get a bad rap. In slower decks, they are often worth a slot, as they help cast expensive spells and can be cracked for a pair of cards later. The non-red ones are better, as the red decks are often lower to the ground, but all of them have their uses.",
                    },
                    new DraftRating
                    {
                        CardName = "Simic Locket",
                        Rating = "2.0",
                        Description = "Lockets get a bad rap. In slower decks, they are often worth a slot, as they help cast expensive spells and can be cracked for a pair of cards later. The non-red ones are better, as the red decks are often lower to the ground, but all of them have their uses.",
                    },
                    new DraftRating
                    {
                        CardName = "Azorius Guildgate",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Gruul Guildgate",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Orzhov Guildgate",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Rakdos Guildgate",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Simic Guildgate",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Hallowed Fountain",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Stomping Ground",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Godless Shrine",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Blood Crypt",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                    new DraftRating
                    {
                        CardName = "Breeding Pool",
                        Rating = "3.0",
                        Description = "If you are two colors, I like picking up Gates over solid playables, but don’t heavily prioritize them. In the Gates deck, they are clearly high picks, and often better than shocklands even.",
                    },
                }},
                { "GRN", new DraftRating[] {
                    new DraftRating
                    {
                        CardName = "Dimir Locket",
                        Rating = "2.0",
                        Description = "I’ve played a few Sealeds, and Lockets have been… fine. They are playable if you have a medium to high curve, and it is nice that you can crack them for two cards later in the game, but I’m still not thrilled to spend 3 mana on them early (which makes them dubious acceleration). I like these quite a bit more than any other 3-mana rocks in the past, so I’m curious to see how people rate them in a month.",
                    },
                    new DraftRating
                    {
                        CardName = "Golgari Locket",
                        Rating = "2.0",
                        Description = "I’ve played a few Sealeds, and Lockets have been… fine. They are playable if you have a medium to high curve, and it is nice that you can crack them for two cards later in the game, but I’m still not thrilled to spend 3 mana on them early (which makes them dubious acceleration). I like these quite a bit more than any other 3-mana rocks in the past, so I’m curious to see how people rate them in a month.",
                    },
                    new DraftRating
                    {
                        CardName = "Izzet Locket",
                        Rating = "2.0",
                        Description = "I’ve played a few Sealeds, and Lockets have been… fine. They are playable if you have a medium to high curve, and it is nice that you can crack them for two cards later in the game, but I’m still not thrilled to spend 3 mana on them early (which makes them dubious acceleration). I like these quite a bit more than any other 3-mana rocks in the past, so I’m curious to see how people rate them in a month.",
                    },
                    new DraftRating
                    {
                        CardName = "Selesnya Locket",
                        Rating = "2.0",
                        Description = "I’ve played a few Sealeds, and Lockets have been… fine. They are playable if you have a medium to high curve, and it is nice that you can crack them for two cards later in the game, but I’m still not thrilled to spend 3 mana on them early (which makes them dubious acceleration). I like these quite a bit more than any other 3-mana rocks in the past, so I’m curious to see how people rate them in a month.",
                    },
                    new DraftRating
                    {
                        CardName = "Boros Guildgate",
                        Rating = "3.0",
                        Description = "If you are 2 colors, I’d take Guildgates reasonably high, over most commons of the same level (especially once you have enough playables). If you have any of the uncommon cycle that costs AABB, these go up a notch, and once you’re 3+ colors they definitely go up. In general, most people probably take these too low, so it’s possible you should think of them as a 3.25, if that helps.",
                    },
                    new DraftRating
                    {
                        CardName = "Dimir Guildgate",
                        Rating = "3.0",
                        Description = "If you are 2 colors, I’d take Guildgates reasonably high, over most commons of the same level (especially once you have enough playables). If you have any of the uncommon cycle that costs AABB, these go up a notch, and once you’re 3+ colors they definitely go up. In general, most people probably take these too low, so it’s possible you should think of them as a 3.25, if that helps.",
                    },
                    new DraftRating
                    {
                        CardName = "Golgari Guildgate",
                        Rating = "3.0",
                        Description = "If you are 2 colors, I’d take Guildgates reasonably high, over most commons of the same level (especially once you have enough playables). If you have any of the uncommon cycle that costs AABB, these go up a notch, and once you’re 3+ colors they definitely go up. In general, most people probably take these too low, so it’s possible you should think of them as a 3.25, if that helps.",
                    },
                    new DraftRating
                    {
                        CardName = "Izzet Guildgate",
                        Rating = "3.0",
                        Description = "If you are 2 colors, I’d take Guildgates reasonably high, over most commons of the same level (especially once you have enough playables). If you have any of the uncommon cycle that costs AABB, these go up a notch, and once you’re 3+ colors they definitely go up. In general, most people probably take these too low, so it’s possible you should think of them as a 3.25, if that helps.",
                    },
                    new DraftRating
                    {
                        CardName = "Selesnya Guildgate",
                        Rating = "3.0",
                        Description = "If you are 2 colors, I’d take Guildgates reasonably high, over most commons of the same level (especially once you have enough playables). If you have any of the uncommon cycle that costs AABB, these go up a notch, and once you’re 3+ colors they definitely go up. In general, most people probably take these too low, so it’s possible you should think of them as a 3.25, if that helps.",
                    },
                }},

                //{ "XLN", new DraftRating[] {
                //}},
                //{ "RIX", new DraftRating[] {
                //}},
                //{ "DOM", new DraftRating[] {
                //}},
                //{ "M19", new DraftRating[] {
                //}},
            };
    }
}
