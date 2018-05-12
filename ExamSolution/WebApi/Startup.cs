using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Data.Common;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using MySql.Data.MySqlClient;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        IServiceCollection _services;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(DbConnection), (IServiceProvider) =>
                InitializeDatabase());
            services.AddMvc();
             _services = services;
        }

        DbConnection InitializeDatabase()
        {
            DbConnection connection;
            string database = Configuration["CloudSQL:Database"];
            switch (database.ToLower())
            {
                case "mysql":
                    connection = NewMysqlConnection();
                    break;
                default:
                    throw new ArgumentException(string.Format(
                        "Invalid database {0}.  Fix appsettings.json.",
                            database), "CloudSQL:Database");
            }
            connection.Open();
            //using (var createTableCommand = connection.CreateCommand())
            //{
            //createTableCommand.CommandText = @"
            //    CREATE TABLE IF NOT EXISTS 
            //    visits (
            //        time_stamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP, 
            //        user_ip CHAR(64)
            //    )";
            //createTableCommand.ExecuteNonQuery();
            //}
            return connection;
        }

        DbConnection NewMysqlConnection()
        {
            // [START mysql_connection]
            var connectionString = new MySqlConnectionStringBuilder(
                Configuration["CloudSql:ConnectionString"])
            {
                SslMode = MySqlSslMode.Required,
                CertificateFile =
                    Configuration["CloudSql:CertificateFile"]
            };
            if (string.IsNullOrEmpty(connectionString.Database))
                connectionString.Database = "users";
            DbConnection connection =
                new MySqlConnection(connectionString.ConnectionString);
            // [END mysql_connection]
            return connection;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
