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
    public class MedicoRepository : IMedicoRepository
    {
        private readonly IDataBaseService _dbConnection;

        public MedicoRepository(IDataBaseService context)
        {
            _dbConnection = context;
        }


        public async Task<int> AddAsync(CadastrarMedicoRequest medico, EPerfil perfil, string senhaHash)
        {
            var sql = @"
            INSERT INTO Usuarios (Nome, CPF, NumeroCRM, Email, SenhaHash, Perfil)
            VALUES (@Nome, @CPF, @NumeroCRM, @Email, @SenhaHash, @perfil);
            SELECT CAST(SCOPE_IDENTITY() as bigint);"; // Retorna o ID gerado           

            var connection = _dbConnection.GetConnection();


            // Executa a consulta e retorna o número de linhas afetadas
            var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new
            {
                Nome = medico.Nome,
                CPF = medico.CPF,
                NumeroCRM = medico.NumeroCRM,
                Email = medico.Email,
                SenhaHash = senhaHash,
                Perfil = (int)perfil
            });

            return result;


        }

        public async Task<Medico> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Medico WHERE Email = @Email;";

            var connection = _dbConnection.GetConnection();



            return await connection.QueryFirstOrDefaultAsync<Medico>(sql, new { Email = email });

        }

        public async Task<Medico> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Medico WHERE Id = @Id;";

            var connection = _dbConnection.GetConnection();

            return await connection.QueryFirstOrDefaultAsync<Medico>(sql, new { Id = id });

        }

        public async Task<int> UpdateAsync(Medico medico)
        {
            var sql = "UPDATE Medico SET Nome = @Nome, CPF = @CPF, NumeroCRM = @NumeroCRM, Email = @Email, SenhaHash = @SenhaHash WHERE Id = @Id;";

            var connection = _dbConnection.GetConnection();

            return await connection.ExecuteAsync(sql, medico);

        }

        public async Task UpdateIdentity(long medicoId, string identity)
        {
            var sql = "Update Usuarios set [Identity]=@identity where Id=@medicoId";

            var connection = _dbConnection.GetConnection();

            await connection.ExecuteAsync(sql, new { medicoId, identity });

        }
    }
}
