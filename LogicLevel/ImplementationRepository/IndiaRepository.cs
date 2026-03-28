using LogicLevel.DefinationRepository;
using ProjectDataStructure.Electronics;
using ProjectDataStructure.Indiaclass.AutoMobile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicLevel.ImplementationRepository
{
    public class IndiaRepository : IIndiaRepository
    {
        private readonly AppDbContext appDbContext;

        public IndiaRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;

        }

        ////Done
        //public async  Task<ServicesOrder> AddServicesOrder(ServicesOrder servicesOrder)
        //{
        //    appDbContext.ServicesOrders.Add(servicesOrder);
        //    appDbContext.SaveChanges();
        //    return servicesOrder;
        //}

        public Task<IEnumerable<ModelYear>> GetVehicleYear()
        {
            throw new NotImplementedException();
        }

        public Task<ServicesOrder> AddServicesOrder(ServicesOrder servicesOrder)
        {
            throw new NotImplementedException();
        }

        //public async  Task<IEnumerable<ModelYear>> GetVehicleYear()
        //{
        //    //IEnumerable<ModelYear> modelYears = appDbContext.modelYears;
        //    //return  modelYears;
        //}


    }
}
