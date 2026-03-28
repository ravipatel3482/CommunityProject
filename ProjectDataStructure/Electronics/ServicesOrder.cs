using ProjectDataStructure.Enum;
using ProjectDataStructure.IdentityClass;
using System.ComponentModel.DataAnnotations;
namespace ProjectDataStructure.Electronics
{
    public class ServicesOrder
    {
        [Key]
        public int ServicesInquiryID { get; set; }
        public string ServicesDescription { get; set; }
        public Servicesstatus Servicesstatus { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}