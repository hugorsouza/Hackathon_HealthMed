using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HackathonHealthMed.Infra.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly IDbConnection _dbConnection;

        public MedicoRepository(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }


        public async Task<int> AddAsync(Medico medico)
        {
            var sql = @"
            INSERT INTO Medico (Nome, CPF, NumeroCRM, Email, SenhaHash)
            VALUES (@Nome, @CPF, @NumeroCRM, @Email, @SenhaHash);
            SELECT CAST(SCOPE_IDENTITY() as int);"; // Retorna o ID gerado

            // O uso de "using" garante que a conexão será fechada após o uso
            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                // Executa a consulta e retorna o número de linhas afetadas
                return await connection.ExecuteAsync(sql, medico);
            }
        }

        public async Task<Medico> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Medico WHERE Email = @Email;";

            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return await _dbConnection.QueryFirstOrDefaultAsync<Medico>(sql, new { Email = email });
            }
        }

        public async Task<Medico> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Medico WHERE Id = @Id;";

            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return await _dbConnection.QueryFirstOrDefaultAsync<Medico>(sql, new { Id = id });
            }
        }

        public async Task<int> UpdateAsync(Medico medico)
        {
            var sql = "UPDATE Medico SET Nome = @Nome, CPF = @CPF, NumeroCRM = @NumeroCRM, Email = @Email, SenhaHash = @SenhaHash WHERE Id = @Id;";

            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return await _dbConnection.ExecuteAsync(sql, medico);
            }
        }
    }
}
