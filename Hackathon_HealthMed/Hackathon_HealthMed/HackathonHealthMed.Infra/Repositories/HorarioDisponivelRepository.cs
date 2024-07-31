using Dapper;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Enums;
using HackathonHealthMed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Infra.Repositories
{
    public class HorarioDisponivelRepository : IHorarioDisponivelRepository
    {
        private readonly IDbConnection _dbConnection;

        public HorarioDisponivelRepository(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
        }

        public async Task<int> ObterPorMedicoAsync(int horarioDisponivel)
        {
            const string query = "SELECT * FROM HorariosDisponiveis WHERE MedicoId = @MedicoId";

            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                // Executa a consulta e retorna o número de linhas afetadas
                return await _dbConnection.ExecuteAsync(query, horarioDisponivel);
            }            
        }

        public async Task<HorarioDisponivel> ObterPorIdAsync(int id)
        {
            const string query = "SELECT * FROM HorariosDisponiveis WHERE Id = @Id";

            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                // Executa a consulta e retorna o número de linhas afetadas
                return await _dbConnection.QuerySingleOrDefaultAsync<HorarioDisponivel>(query, new { Id = id });
            }
            
        }

        public async Task<bool> AdicionarAsync(HorarioDisponivel horarioDisponivel)
        {
            const string query = @"
            INSERT INTO HorariosDisponiveis (MedicoId, Data, HoraInicio, HoraFim, EstaDisponivel)
            VALUES (@MedicoId, @Data, @HoraInicio, @HoraFim, @Disponivel)";
            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                // Executa a consulta e retorna o número de linhas afetadas
                // Executa a consulta e obtém o número de linhas afetadas
                var rowsAffected = await connection.ExecuteAsync(query, horarioDisponivel);

                // Retorna true se pelo menos uma linha foi afetada, caso contrário, retorna false
                return rowsAffected > 0;
            }
        }

        public async Task AtualizarAsync(HorarioDisponivel horarioDisponivel)
        {
            const string query = @"
            UPDATE HorariosDisponiveis
            SET Data = @Data, HoraInicio = @HoraInicio, HoraFim = @HoraFim, EstaDisponivel = @EstaDisponivel
            WHERE Id = @Id";

            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                // Executa a consulta e retorna o número de linhas afetadas
                await _dbConnection.ExecuteAsync(query, horarioDisponivel);
            }
           
        }

        public async Task<bool> VerificarDisponibilidadeAsync(int medicoId, DateTime data, TimeSpan horaInicio)
        {
            const string query = @"
            SELECT COUNT(1) 
            FROM HorariosDisponiveis
            WHERE MedicoId = @MedicoId AND Data = @Data AND HoraInicio = @HoraInicio AND Disponivel = 1";

            using (var connection = _dbConnection)
            {
                // Abre a conexão se necessário
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                // Executa a consulta e retorna o número de linhas afetadas
                var count = await _dbConnection.ExecuteScalarAsync<int>(query, new { MedicoId = medicoId, Data = data, HoraInicio = horaInicio });
                return count == 0;
            }
            
        }

    }

}
