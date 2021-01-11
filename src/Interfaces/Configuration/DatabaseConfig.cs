namespace PhotoSi.Interfaces.Configuration
{
    public class DatabaseConfig
    {
        public string DbType { get; set; }
        public string ConnectionString { get; set; }
        public bool UseMigrationScript { get; set; }
    }
}