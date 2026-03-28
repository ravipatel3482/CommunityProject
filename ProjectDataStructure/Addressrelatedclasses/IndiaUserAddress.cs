using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectDataStructure.Addressrelatedclasses
{
    [Table("IndianUserAddresses")]
    public class IndiaUserAddress
    {
        [Key]
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        [ForeignKey("CityId")]
        [Required]
        public IndiaCity City { get; set; }

    }

}
