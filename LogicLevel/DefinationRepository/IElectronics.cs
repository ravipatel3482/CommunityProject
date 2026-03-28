using ProjectDataStructure.Enum;
using ProjectDataStructure.IndiaViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicLevel.DefinationRepository
{
    public interface IElectronics
    {

        Task<int> Saveserviceorder(ServicesOrderViewModel servicesOrder);
        Task<int> SaveServiceProvider(ServiceproviderViewModel serviceprovider);
        Task<IEnumerable<ServiceProviderDisplayViewModel>> GetAllServicesProvider();
        Task<IEnumerable<ServicesOrderViewModel>> GetAllServiceInquiry();
        void UpdateServcieStatus(int Id, int servicesstatus);
    }
}
