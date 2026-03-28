using ProjectDataStructure.Electronics;
using ProjectDataStructure.Indiaclass.AutoMobile;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicLevel.DefinationRepository
{
    public interface IIndiaRepository
    {
        Task<ServicesOrder> AddServicesOrder(ServicesOrder servicesOrder);
        Task<IEnumerable<ModelYear>> GetVehicleYear();

    }
}
