using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class MarsContextSeed
{
    public static async Task SeedAsync(MarsContext marsContext, ILogger logger)
    {
        try
        {
            marsContext.Database.Migrate();

            if (!await marsContext.Posts.AnyAsync())
            {
                await marsContext.Posts.AddRangeAsync(GetPreconfiguredPosts());

                await marsContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }


    private static List<Post> GetPreconfiguredPosts()
    {
        var posts = new List<Post>
        {
            new("Mars Had Water In The Ancient Past",
                "We’ve been debating for centuries about whether Mars had life or not. In fact, the astronomer " +
                "Percival Lowell misinterpreted observations of “canali” — the Italian word for channels — on the planet " +
                "as evidence of alien-made canals. It turned out Lowell’s observations were hampered by poor telescope " +
                "optics of his day, and the canals he saw were optical illusions. That said, several spacecraft have spotted " +
                "other signs of ancient water — channels grooved in the terrain and rocks that only could have formed in the " +
                "presence of water, for example.",
                1),
            new("Mars Has Two Moons – And One Of Them Is Doomed",
                "The planet has two asteroid-like moons called Phobos and Deimos. Because they have " +
                "compositions that are similar to asteroids found elsewhere in the Solar System, according to NASA, " +
                "most scientists believe the Red Planet’s gravity snatched the moons long ago and forced them into " +
                "orbit. But in the life of the Solar System, Phobos has a pretty short lifetime. In about 30 million " +
                "to 50 million years, Phobos is going to crash into Mars’ surface or rip apart because the tidal force " +
                "of the planet will prove too much to resist.",
                1),
            new("Mars Would Kill An Unprotected Astronaut Quickly",
                "There are a lot of unpleasant scenarios for somebody who took of their helmet. First, Mars " +
                "is usually pretty cold; its average temperature is -50 degrees Fahrenheit (-45 degrees Celsius) at the " +
                "mid-latitudes. Second, it has practically no atmosphere. The air pressure on Mars is only 1% of what we " +
                "have (on average) on the Earth’s surface. And third, even if it did have atmosphere, the composition is " +
                "not compatible with the nitrogen-oxygen mix humans require. Specifically, Mars has about 95% carbon " +
                "dioxide, 3% nitrogen, 1.6% argon and a few other elements in its atmosphere.",
                1)
        };

        return posts;
    }
}