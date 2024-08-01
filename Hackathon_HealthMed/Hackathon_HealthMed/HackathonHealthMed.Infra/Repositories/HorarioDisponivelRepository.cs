using Dapper;
using HackathonHealthMed.Domain.Entities;
using HackathonHealthMed.Domain.Interfaces;
using HackathonHealthMed.Infra.Context;
using static Dapper.SqlMapper;

namespace HackathonHealthMed.Infra.Repositories
{
    public class HorarioDisponivelRepository : IHorarioDisponivelRepository
    {
        private readonly IDataBaseService _dbConnection;

        public HorarioDisponivelRepository(IDataBaseService context)
        {
            _dbConnection = context;
        }

        public async Task<HorarioDisponivel> ObterPorMedicoAsync(int medicoId)
        {
            var sql = "select * from Agenda where MedicoId = @medicoId and horario is not null";
            var connection = _dbConnection.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<HorarioDisponivel>(sql, new { MedicoId = medicoId });
        }

        public async Task<HorarioDisponivel> ObterPorIdAsync(int id)
        {
            var sql = "select * from Agenda where Id = @Id";

            var connection = _dbConnection.GetConnection();

            return await connection.QuerySingleOrDefaultAsync<HorarioDisponivel>(sql, new { Id = id });
        }

        public async Task<bool> AdicionarAsync(HorarioDisponivel horarioDisponivel)
        {
            var sql = @"insert into agenda (MedicoId, PacienteID, Horario) 
                        values (@MedicoId, null,  @Horario);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);";
           
            var connection = _dbConnection.GetConnection();

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                horarioDisponivel.MedicoId,
                horarioDisponivel.Horario
            });

            // Retorne true se pelo menos uma linha foi afetada, caso contrário, false
            return rowsAffected > 0;
        }

        public async Task AtualizarAsync(HorarioDisponivel horarioDisponivel)
        {
            var sql = "update Agenda set horario = @horario where id = @Id";

            var connection = _dbConnection.GetConnection();

           await connection.ExecuteAsync(sql, horarioDisponivel);          
        }

        public async Task<bool> VerificarDisponibilidadeAsync(int medicoId, DateTime horario)
        {
            var sql= "select * from Agenda where MedicoId = @medicoId and horario is null";

            var connection = _dbConnection.GetConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new { MedicoId = medicoId, Data = horario});
                return count == 0;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var sql = "delete Agenda where id = @id";

            var connection = _dbConnection.GetConnection();
            
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<HorarioDisponivel>> ObterPorNomeMedicoAsync(string nome)
        {
            var sql = @"
                      select A.MedicoId, U.Nome, A.Horario, A.Id  
                      from Usuarios U
                      inner join Agenda A on U.Id = A.MedicoId 
                      WHERE U.Nome LIKE @Nome";

            var connection = _dbConnection.GetConnection();

            return await connection.QueryAsync<HorarioDisponivel>(sql, new { Nome = $"%{nome}%" });
        }

        public async Task<bool> AgendarConsultaAsync(HorarioDisponivel consulta)
        {
            var sql = "update Agenda set pacienteId = @pacienteId where id = @Id";

            var connection = _dbConnection.GetConnection();

            var result = await connection.ExecuteAsync(sql, consulta);
                return result > 0;        
        }

        public async Task<HorarioDisponivel> ObterPacientePorIdAsync(int pacienteId)
        {
            var sql = "select * from Usuarios where Id = @Id";

            var connection = _dbConnection.GetConnection();

            return await connection.QuerySingleOrDefaultAsync<HorarioDisponivel>(sql, new { Id = pacienteId });
        }

        public async Task<bool> VerificarDisponibilidadeConsultaAsync(int medicoId, int pacienteId)
        {
            var sql = "select * from Agenda where MedicoId = @medicoId and @pacienteId is null";

            var connection = _dbConnection.GetConnection();

            var count = await connection.ExecuteScalarAsync<int>(sql, new { MedicoId = medicoId, PacienteId = pacienteId });
            return count == 0;
        }

        public async Task<bool> CancelarConsultaAsync(int id, int pacienteId)
        {
            var sql = "update Agenda set pacienteId = null where id = @id and pacienteId = @pacienteId";

            var connection = _dbConnection.GetConnection();

            var result = await connection.ExecuteAsync(sql, new { Id = id, PacienteId = pacienteId });
            return result > 0;
        }

        public async Task<HorarioDisponivel> ExibirConsultasAsync(int pacienteId)
        {

            var sql = "select * from Agenda where PacienteId = @pacienteId";
            var connection = _dbConnection.GetConnection();
            return await connection.QueryFirstOrDefaultAsync<HorarioDisponivel>(sql, new { PacienteId = pacienteId });
        }

        public Task<int> AddAsync(HorarioDisponivel horario)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(HorarioDisponivel horario)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HorarioDisponivel>> GetByMedicoIdAsync(int medicoId)
        {
            throw new NotImplementedException();
        }
    }
}

