

using Microsoft.Extensions.DependencyInjection;
using MTGAHelper.Lib.Scraping.DeckSources.MtgGoldfish;

namespace MTGAHelper.Lib.Scraping.DeckSources.IoC
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection RegisterServicesScrapingDeckSources(this IServiceCollection services)
        {
            return services
                .AddTransient<MtgTop8Scraper>()
                .AddTransient<MtgaToolScraper>()
                .AddTransient<MtgGoldfishTournamentsScraper>()
            ;
        }
    }
}
