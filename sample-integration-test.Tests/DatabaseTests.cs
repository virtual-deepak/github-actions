using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sample_integration_test.Tests
{
    [TestClass]
    public class DatabaseTests: IDisposable
    {
        MusicCatalogContext context;

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

        public DatabaseTests()
        {
            var config = InitConfiguration();

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<MusicCatalogContext>();

            // build connection string depending on security model
            string connectionString = "";
            string databaseName = "musiccatalogtests_" + Guid.NewGuid();

            if (config["Database:IntegratedSecurity"] == "true")
            {
                // using Integrated Security for local testing
                Console.WriteLine("Using local SQL Server...");

                connectionString = $"Server=(localdb)\\mssqllocaldb;Database=" + databaseName + ";Trusted_Connection=True;MultipleActiveResultSets=true";
                Console.WriteLine("Connection String to remoted SQL Server: " + connectionString);
            }
            else
            {
                // using SQL authentication (username/password)  for remote database (GitHub Actions Service Container)
                Console.WriteLine("Using Remote SQL Server...");

                var serverName = config["Database:Server"] + "," + config["Database:Port"];
                var userName = config["Database:UserId"];
                var password = config["Database:Password"];
                connectionString = "Server=" + serverName + ";Database=" + databaseName + ";User Id=" + userName + ";Password=" + password;

                Console.WriteLine("Connection String to remoted SQL Server: " + connectionString);
            }

            builder.UseSqlServer(connectionString)
                .UseInternalServiceProvider(serviceProvider);

            context = new MusicCatalogContext(builder.Options);
            context.Database.EnsureCreated();
            DbInitializer.Initialize(context);

            Console.WriteLine("Database successfully initialized.  Database name: " + databaseName);
        }

        [TestMethod]
        public async Task QueryArtistsTest()
        {
            Console.WriteLine("Starting QueryArtistsTest...");

            var artistResults = await context.Artists.ToListAsync();

            Console.WriteLine("Results returned: " + artistResults.Count);
            Console.WriteLine("Name of first artist: " + artistResults[0].ArtistName);

            Assert.AreEqual(4, artistResults.Count);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
        }
    }
}