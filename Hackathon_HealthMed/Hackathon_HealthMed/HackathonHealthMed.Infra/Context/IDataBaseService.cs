using System.Data;

namespace HackathonHealthMed.Infra.Context;
    public interface IDataBaseService
    {
        public IDbConnection GetConnection();
    }
