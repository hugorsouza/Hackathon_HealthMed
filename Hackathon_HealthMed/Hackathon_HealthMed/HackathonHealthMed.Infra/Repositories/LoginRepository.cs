using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Enums;
using HackathonHealthMed.Infra.Context;

namespace HackathonHealthMed.Infra.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IDataBaseService _dbConnection;

        public LoginRepository(IDataBaseService context)
        {
            _dbConnection = context;
        }



        public async Task<Medico> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM USUARIOS WHERE Email = @Email;";

            var connection = _dbConnection.GetConnection();



            return await connection.QueryFirstOrDefaultAsync<Medico>(sql, new { Email = email });

        }


        public async Task<Usuario> GetByIdentityAsync(string token)
        {
            var sql = @$"SELECT * FROM USUARIOS WHERE [IDENTITY] LIKE '%{token}%'";

            var connection = _dbConnection.GetConnection();

            try
            {
                var result = await connection.QueryAsync<Usuario>(sql);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return null;
            }


            

            

        }


    }
}
