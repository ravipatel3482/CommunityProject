using Dapper;
using LogicLevel.DefinationRepository;
using ProjectDataStructure.IndiaViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LogicLevel.ImplementationRepository
{
    public class Electronics : IElectronics
    {
        private readonly IUnitofWork unitofWork;
        private readonly AppDbContext dbContext;
        public Electronics(IUnitofWork unitofWork, AppDbContext dbContext)
        {
            this.unitofWork = unitofWork;
            this.dbContext = dbContext;

        }

        public async Task<int> SaveServiceProvider(ServiceproviderViewModel serviceprovider)
        {
            try
            {
                var Connection = unitofWork.GetConnection();
                var Paramaters = new DynamicParameters();
                int counter = 1;
                foreach (var provider_services in serviceprovider.ServicesTypes)
                {
                    if (provider_services.isSelected == true)
                    {
                        Paramaters.Add("@ServiceTypeId" + counter, provider_services.ServicesTypeId);
                    }
                    else
                    {
                        Paramaters.Add("@ServiceTypeId" + counter, null);
                    }
                    counter++;
                }
                Paramaters.Add("@ApplicationUserId", serviceprovider.ApplicationUserId);
                Paramaters.Add("@AddressId", serviceprovider.AddressId);
                Paramaters.Add("@FirstName", serviceprovider.FirstName);
                Paramaters.Add("@Email", serviceprovider.Email);
                Paramaters.Add("@phoneNumber", serviceprovider.phoneNumber);
                Paramaters.Add("@BusinessName", serviceprovider.BusinessName);
                Paramaters.Add("@Address", serviceprovider.BusinessAddress);
                Paramaters.Add("@City", serviceprovider.City);
                Paramaters.Add("@StateId", serviceprovider.stateID);
                Paramaters.Add("@Zipcode ", serviceprovider.ZipCode);
                Paramaters.Add("@KindofServices", serviceprovider.Service);
                Paramaters.Add("@Photo", serviceprovider.photopath);
                Paramaters.Add("@ProviderIdTo", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result =Connection.Query("SpsaveProviderData", Paramaters, commandType: CommandType.StoredProcedure);
                Connection.Close();
                int ProviderId = Paramaters.Get<int>("@ProviderIdTo");
                return await Task.FromResult(ProviderId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Saveserviceorder(ServicesOrderViewModel servicesOrder)
        {
            try
            {
                var Paramaters = new DynamicParameters();
                Paramaters.Add("@ApplicationUSerID", servicesOrder.ApplicationUserId);
                Paramaters.Add("@FirstName", servicesOrder.Name);
                Paramaters.Add("@Email", servicesOrder.Email);
                Paramaters.Add("@phoneNumber", servicesOrder.PhoneNo);
                Paramaters.Add("@Address", servicesOrder.ServicesAddress);
                Paramaters.Add("@Zipcode", servicesOrder.Zipcode);
                Paramaters.Add("@CityId", servicesOrder.CityId);
                Paramaters.Add("@AddressId", servicesOrder.AddressId);                
                Paramaters.Add("@ServicesStatus",servicesOrder.Servicesstatus);
                Paramaters.Add("@ServicesDescription", servicesOrder.ServicesDescription);
                Paramaters.Add("@ServicesInquiryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var Connection = unitofWork.GetConnection();
                var result = await Connection.QueryAsync<ServicesOrderViewModel>("SpSaveServicesInquiry", Paramaters, commandType: CommandType.StoredProcedure);
                Connection.Close();
                int InquiryId = Paramaters.Get<int>("@ServicesInquiryId");
                //string UserId = Paramaters.Get<string>("@UserId");
                //Dictionary<string, int> UserIdAndServicesId = new Dictionary<string, int>();
                //UserIdAndServicesId.Add(UserId, InquiryId);
                return InquiryId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ServiceProviderDisplayViewModel>> GetAllServicesProvider()

        {
            try
            {
                var Connection = unitofWork.GetConnection();
                //var Paramaters = new DynamicParameters();
                //Paramaters.Add("@SearchTerm", SearchTerm);
                var result = await Connection.QueryAsync<ServiceProviderDisplayViewModel>("SpGetServicesProvider", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ServicesOrderViewModel>> GetAllServiceInquiry()
        {
            try
            {
                var Connection = unitofWork.GetConnection();
                //var Paramaters = new DynamicParameters();
                //Paramaters.Add("@SearchTerm", SearchTerm);
                var result = await Connection.QueryAsync<ServicesOrderViewModel>("SpGetServicesInquiry", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateServcieStatus(int Id, int servicesstatus)
        {
            try
            {
                var Connection = unitofWork.GetConnection();
                var Paramaters = new DynamicParameters();
                Paramaters.Add("@SerViceInquiryId",Id);
                Paramaters.Add("@ServiceStatus", servicesstatus);
                Connection.Query("SpUpdateserviceInquiry", Paramaters, commandType: CommandType.StoredProcedure);
                
            }
            catch (Exception e)
            {
                throw;
            }

        }
    }
}
