using ProjectDataStructure.Addressrelatedclasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicLevel.DefinationRepository
{
    public interface IBaseAtionRepository
    {
        IEnumerable<IndiaCity> GetCityList();
        Task<IndiaUserAddress> GetUserAddressAsync(string Id);
    }
}
