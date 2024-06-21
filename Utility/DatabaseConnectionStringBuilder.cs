namespace PIEAPI.Utility
{
    public class DatabaseConnectionStringBuilder
    {
        public static string GetSqlConnectionString(IConfiguration config)
        {
            return $"Host={config["PIEADatabaseConnection_Server"]};Username={config["PIEADatabaseConnection_Username"]};Password={config["PIEADatabaseConnection_Password"]};Database={config["PIEADatabaseConnection_Database"]}"; 
        }
    }
}
