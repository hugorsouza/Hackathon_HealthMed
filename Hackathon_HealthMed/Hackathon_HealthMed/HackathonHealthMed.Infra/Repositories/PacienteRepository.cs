using Dapper;
using HackathonHealthMed.Application.Request;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Enums;
using HackathonHealthMed.Domain.Interfaces;
using HackathonHealthMed.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Infra.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {

        private readonly IDataBaseService _dbConnection;

        public PacienteRepository(IDataBaseService context)
        {
            _dbConnection = context;
        }
        public Task<int> AddAsync(Paciente paciente)
        {
            throw new NotImplementedException();
        }



        public Task<Paciente> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateIdentity(long pacienteId, string identity)
        {
            var sql = "Update Usuarios set [Identity]=@identity where Id=@pacienteId";

            var connection = _dbConnection.GetConnection();

            await connection.ExecuteAsync(sql, new { pacienteId, identity });

        }

        public async Task<int> AddAsync(CadastrarPacienteRequest paciente, EPerfil perfil, string senhaHash)
        {
            var sql = @"
            INSERT INTO Usuarios (Nome, CPF, Email, SenhaHash, Perfil)
            VALUES (@Nome, @CPF,  @Email, @SenhaHash, @perfil);
            SELECT CAST(SCOPE_IDENTITY() as bigint);"; // Retorna o ID gerado           

            var connection = _dbConnection.GetConnection();


            // Executa a consulta e retorna o número de linhas afetadas
            var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new
            {
                Nome = paciente.Nome,
                CPF = paciente.CPF,
                Email = paciente.Email,
                SenhaHash = senhaHash,
                Perfil = (int)perfil
            });

            return result;
        }
    }
}
