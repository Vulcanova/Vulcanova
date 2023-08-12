using LiteDB.Async;

namespace Vulcanova.Core.Data;

public static class Config
{
    public static void RegisterLiteDb(this IContainerRegistry containerRegistry)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "data.db");
        var db = new LiteDatabaseAsync(dbPath);
        
        LiteDbMigrator.Migrate(db.UnderlyingDatabase);

        containerRegistry.RegisterInstance(typeof(LiteDatabaseAsync), db);
    }
}