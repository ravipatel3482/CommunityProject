using Microsoft.AspNetCore.Identity;
using ProjectDataStructure.Addressrelatedclasses;
using ProjectDataStructure.Electronics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectDataStructure.IdentityClass
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public string EncryptedId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IndiaUserAddress IndiaUserAddress { get; set; }
        public IEnumerable<ServicesOrder> servicesOrder { get; set; }
        public ServicesProvider ServicesProvider { get; set; }
        
    }
    public class ReusableApplicationUser : ApplicationUser
    {
        public int IndiaUserAddressAddressId { get; set; }
    }
}
