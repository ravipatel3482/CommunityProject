using ProjectDataStructure.Enum;
namespace ProjectDataStructure.Electronics
{
    public class ProviderServices
    {


        public int ProviderId { get; set; }
        public int ServicesTypeId { get; set; }
        //public  virtual  ServicesProvider servicesProvider { get; set; }
        public virtual ServicesType servicesType { get; set; }
    }
}
