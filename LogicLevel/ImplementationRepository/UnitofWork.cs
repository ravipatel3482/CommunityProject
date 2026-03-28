using LogicLevel.DefinationRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LogicLevel.ImplementationRepository
{
    public class UnitofWork : IUnitofWork, IDisposable
    {
        private readonly IConfiguration config;
        private SqlConnection SqlConnection;
        private SqlTransaction SqlTransaction;

        public UnitofWork(IConfiguration config)
        {
            this.config = config;

        }

        public SqlConnection GetConnection()
        {
            if (SqlConnection != null)
            {
                if (SqlConnection.State == ConnectionState.Open)
                {
                    return SqlConnection;
                }
            }
            SqlConnection = new SqlConnection(config["DBConnection"]);
            SqlConnection.Open();
            return SqlConnection;

        }
        public SqlTransaction GetTransaction(SqlConnection SqlConnection)
        {
            if (SqlConnection.State == ConnectionState.Open)

                SqlTransaction = SqlConnection.BeginTransaction();


            return SqlTransaction;
        }
        public void CommitChange()
        {
            try
            {
                SqlTransaction.Commit();
            }
            catch
            {
                SqlTransaction.Rollback();
            }
            finally
            {
                SqlTransaction.Dispose();
                SqlConnection.Close();
            }
        }
        public void RollBackChanges()
        {
            try
            {
                SqlTransaction.Rollback();
            }
            catch (Exception)
            {


            }
            finally
            {
                SqlTransaction.Dispose();
                SqlConnection.Close();
            }
        }
        public void Dispose()
        {
            if (SqlTransaction != null)
            {
                SqlTransaction.Dispose();
            }
            if (SqlConnection != null)
            {
                SqlConnection.Close();
            }

        }



    }
}
