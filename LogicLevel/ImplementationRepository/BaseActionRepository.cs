using Dapper;
using LogicLevel.DefinationRepository;
using ProjectDataStructure.Addressrelatedclasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LogicLevel.ImplementationRepository
{
    public class BaseActionRepository : IBaseAtionRepository
    {
        private readonly IUnitofWork unitofWork;
        private readonly AppDbContext appDbContext;

        public BaseActionRepository(IUnitofWork unitofWork, AppDbContext appDbContext)
        {
            this.unitofWork = unitofWork;
            this.appDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

        public IEnumerable<IndiaCity> GetCityList()
        {

            return appDbContext.IndiaCities;
        }

        public async Task<IndiaUserAddress> GetUserAddressAsync(string Id)
        {
            try
            {
                var Connection = unitofWork.GetConnection();
                var Paramaters = new DynamicParameters();
                Paramaters.Add("@UserId", Id);
                var result = await Connection.QueryAsync<IndiaUserAddress>("SpGetEmployeeAddress", Paramaters, commandType: CommandType.StoredProcedure);
                Connection.Close();
                return result.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
