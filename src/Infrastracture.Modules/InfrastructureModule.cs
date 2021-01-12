using DB.EFCore.Context;
using DB.EFCore.Repositories;
using DomainModel.Interfaces;
using FakeSendMail;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoSi.Interfaces;
using PhotoSi.Interfaces.Configuration;
using System;
using System.IO;

namespace Infrastracture.Modules
{
    public class InfrastructureModule
    {
        public static void ConfiguresInfrastructure(IServiceCollection services, IConfiguration configuration, string contentRootPath)
        {
            //Database
            var dbType = configuration.GetSection("Database:DbType").Value;
            var connectionString = configuration.GetSection("Database:ConnectionString").Value;

            if (dbType != null &&
                dbType.Equals("SQLite", StringComparison.InvariantCultureIgnoreCase))
            {
                var stringBuilder = new SqliteConnectionStringBuilder
                {
                    ConnectionString = connectionString
                };

                stringBuilder.DataSource = Path.Combine(contentRootPath, stringBuilder.DataSource);

                services.AddDbContext<DatabaseContext>(option =>
                    option.UseLazyLoadingProxies()
                        .UseSqlite(stringBuilder.ConnectionString));
            }
            else if (dbType != null &&
                    dbType.Equals("SqlServer", StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddDbContext<DatabaseContext>(option =>
                    option.UseLazyLoadingProxies()
                        .UseSqlServer(connectionString));
            }

            //Repository
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IProductRepository, ProductRepository>();

            //Fake Mail
            services.AddSingleton<ISendMail, SendMail>();
        }
    }
}
