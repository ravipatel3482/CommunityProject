using Dapper;
using LogicLevel.DefinationRepository;
using ProjectDataStructure.IdentityClass;
using System;
using System.Data;

namespace LogicLevel.ImplementationRepository
{
    public class AccountManager : IAccountManager
    {
        private readonly IUnitofWork unitofWork;
        public AccountManager(IUnitofWork unitofWork)
        {
            this.unitofWork = unitofWork;
        }
        public void SaveUserRole(AspNetRoles aspNetRoles)
        {
            try
            {
                var Connection = unitofWork.GetConnection();
                var Paramaters = new DynamicParameters();
                Paramaters.Add("@RollId", aspNetRoles.RoleId);
                Paramaters.Add("@USerId", aspNetRoles.USerId);
                Connection.QueryAsync("SpSaveUserRole", Paramaters, commandType: CommandType.StoredProcedure);
                Connection.Close();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
