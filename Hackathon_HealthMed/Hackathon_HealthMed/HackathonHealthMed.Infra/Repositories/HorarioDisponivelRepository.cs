using Dapper;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Enums;
using HackathonHealthMed.Domain.Interfaces;
using HackathonHealthMed.Infra.Context;
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
        private readonly IDataBaseService _dbConnection;

        public HorarioDisponivelRepository(IDataBaseService context)
        {
            _dbConnection = context;
        }

        public async Task<int> ObterPorMedicoAsync(int horarioDisponivel)
        {
            var sql = "SELECT * FROM HorariosDisponiveis WHERE MedicoId = @MedicoId";

            var connection = _dbConnection.GetConnection();
            // Executa a consulta e retorna o número de linhas afetadas
            return await connection.ExecuteAsync(sql, horarioDisponivel);
        }

        public async Task<HorarioDisponivel> ObterPorIdAsync(int id)
        {
            var sql = "SELECT * FROM HorariosDisponiveis WHERE Id = @Id";

            var connection = _dbConnection.GetConnection();

            return await connection.QuerySingleOrDefaultAsync<HorarioDisponivel>(sql, new { Id = id });
          
            
        }

        public async Task<bool> AdicionarAsync(HorarioDisponivel horarioDisponivel)
        {
            var sql = @"
            INSERT INTO HorariosDisponiveis (MedicoId, Data, HoraInicio, HoraFim, EstaDisponivel)
            VALUES (@MedicoId, @Data, @HoraInicio, @HoraFim, @Disponivel)";
           
            var connection = _dbConnection.GetConnection();

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                horarioDisponivel.MedicoId,
                horarioDisponivel.Data,
                horarioDisponivel.HoraInicio,
                horarioDisponivel.HoraFim,
                horarioDisponivel.EstaDisponivel
            });

            // Retorne true se pelo menos uma linha foi afetada, caso contrário, false
            return rowsAffected > 0;
        }

        public async Task AtualizarAsync(HorarioDisponivel horarioDisponivel)
        {
            var sql = @"
            UPDATE HorariosDisponiveis
            SET Data = @Data, HoraInicio = @HoraInicio, HoraFim = @HoraFim, EstaDisponivel = @EstaDisponivel
            WHERE Id = @Id";

            var connection = _dbConnection.GetConnection();

           await connection.ExecuteAsync(sql, horarioDisponivel);          
        }

        public async Task<bool> VerificarDisponibilidadeAsync(int medicoId, DateTime data, TimeSpan horaInicio)
        {
            var sql= @"
            SELECT COUNT(1) 
            FROM HorariosDisponiveis
            WHERE MedicoId = @MedicoId AND Data = @Data AND HoraInicio = @HoraInicio AND Disponivel = 1";

            var connection = _dbConnection.GetConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new { MedicoId = medicoId, Data = data, HoraInicio = horaInicio });
                return count == 0;
        }    
    }
}

