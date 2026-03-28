using System.Data.SqlClient;

namespace LogicLevel.DefinationRepository
{
    public interface IUnitofWork
    {
        SqlConnection GetConnection();
        void RollBackChanges();
        void CommitChange();
        SqlTransaction GetTransaction(SqlConnection SqlConnection);

    }
}
