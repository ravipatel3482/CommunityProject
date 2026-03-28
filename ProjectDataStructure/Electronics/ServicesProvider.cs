using ProjectDataStructure.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDataStructure.Electronics
{
    public class ServicesProvider
    {
        public ServicesProvider()
        {
            this.ServicesTypes = new HashSet<ServicesType>();
        }
        [Key]
        public int ProviderId { get; set; }
        public string ServiceYouProvide { get; set; }
        public string BusinessName { get; set; }
        public string BusinessPhotoPath { get; set; }
        public virtual ICollection<ServicesType> ServicesTypes { get; set; }
    }
}
