using ProjectDataStructure.Electronics;
using System.Collections.Generic;

namespace ProjectDataStructure.Enum
{
    public class ServicesType
    {
        public ServicesType()
        {
            this.ServicesProviders = new HashSet<ServicesProvider>();
        }

        public int ServicesTypeId { get; set; }
        public string ServiceType { get; set; }
        public virtual ICollection<ServicesProvider> ServicesProviders { get; set; }
    }
    public class ServicesTypesViewModel : ServicesType
    {
        public bool isSelected { get; set; }
    }
}
