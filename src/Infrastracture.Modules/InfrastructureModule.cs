using DB.Dapper;
using DB.EFCore.Context;
using Interfaces.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Infrastracture.Modules
{
    public class InfrastructureModule
    {
        public static void ConfigureOthersInfrastructure(IServiceCollection services, IConfiguration configuration, string contentRootPath)
        {
            var databaseConfig = configuration.GetSection("Database") as DatabaseConfig;

            //services.AddTransient(sp => ConnectionFactory.CreateDbConnection(databaseConfig));

            if (databaseConfig.DbType != null &&
                databaseConfig.DbType.Equals("SQLite", StringComparison.InvariantCultureIgnoreCase))
            {
                var stringBuilder = new SqliteConnectionStringBuilder
                {
                    ConnectionString = databaseConfig.ConnectionString
                };

                stringBuilder.DataSource = Path.Combine(contentRootPath, stringBuilder.DataSource);

                services.AddDbContext<DatabaseContext>(option =>
                    option.UseLazyLoadingProxies()
                        .UseSqlite(stringBuilder.ConnectionString));
            }
            else if (databaseConfig.DbType != null &&
                    databaseConfig.DbType.Equals("SqlServer", StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddDbContext<DatabaseContext>(option =>
                    option.UseLazyLoadingProxies()
                        .UseSqlServer(databaseConfig.ConnectionString));
            }

        }
    }
}
