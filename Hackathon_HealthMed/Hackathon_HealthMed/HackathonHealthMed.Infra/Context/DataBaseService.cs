
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace HackathonHealthMed.Infra.Context
{
    public class DataBaseService : IDataBaseService
    {
        public DataBaseService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public IDbConnection GetConnection()
        {
            var sqlConnectionStringName = Configuration.GetConnectionString("Ecommerce");
            return new SqlConnection(sqlConnectionStringName);
        }
    }
}
