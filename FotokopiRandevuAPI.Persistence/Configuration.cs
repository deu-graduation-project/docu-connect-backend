using DotNetEnv;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotokopiRandevuAPI.Persistence
{
    static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                Env.Load();
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSQL");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string is not set in environment variables.");
                }

                return connectionString;
            }
        }
    }
}
