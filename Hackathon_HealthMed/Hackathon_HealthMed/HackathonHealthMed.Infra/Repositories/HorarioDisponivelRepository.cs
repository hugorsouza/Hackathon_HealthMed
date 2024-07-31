using Dapper;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonHealthMed.Infra.Repositories
{
    public class HorarioDisponivelRepository : IHorarioDisponivelRepository
    {
        private readonly IDbConnection _dbConnection;

        public HorarioDisponivelRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<HorarioDisponivel>> ObterPorMedicoAsync(int medicoId)
        {
            const string query = "SELECT * FROM HorariosDisponiveis WHERE MedicoId = @MedicoId";
            return await _dbConnection.QueryAsync<HorarioDisponivel>(query, new { MedicoId = medicoId });
        }

        public async Task<HorarioDisponivel> ObterPorIdAsync(int id)
        {
            const string query = "SELECT * FROM HorariosDisponiveis WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<HorarioDisponivel>(query, new { Id = id });
        }

        public async Task AdicionarAsync(HorarioDisponivel horarioDisponivel)
        {
            const string query = @"
            INSERT INTO HorariosDisponiveis (MedicoId, Data, HoraInicio, HoraFim, EstaDisponivel)
            VALUES (@MedicoId, @Data, @HoraInicio, @HoraFim, @Disponivel)";
            await _dbConnection.ExecuteAsync(query, horarioDisponivel);
        }

        public async Task AtualizarAsync(HorarioDisponivel horarioDisponivel)
        {
            const string query = @"
            UPDATE HorariosDisponiveis
            SET Data = @Data, HoraInicio = @HoraInicio, HoraFim = @HoraFim, EstaDisponivel = @EstaDisponivel
            WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(query, horarioDisponivel);
        }

        public async Task<bool> VerificarDisponibilidadeAsync(int medicoId, DateTime data, TimeSpan horaInicio)
        {
            const string query = @"
            SELECT COUNT(1) 
            FROM HorariosDisponiveis
            WHERE MedicoId = @MedicoId AND Data = @Data AND HoraInicio = @HoraInicio AND Disponivel = 1";
            var count = await _dbConnection.ExecuteScalarAsync<int>(query, new { MedicoId = medicoId, Data = data, HoraInicio = horaInicio });
            return count == 0;
        }
    }

}
