namespace PIEAPI.Utility
{
    public class DatabaseConnectionStringBuilder
    {
        public static string GetSqlConnectionString(IConfiguration config)
        {
            return $"Host={config["PIEDatabaseConnection_Server"]};Username={config["PIEDatabaseConnection_Username"]};Password={config["PIEDatabaseConnection_Password"]};Database={config["PIEDatabaseConnection_Database"]}"; 
        }
    }
}
