using ProjectDataStructure.Enum;
using System.ComponentModel.DataAnnotations;

namespace ProjectDataStructure.Addressrelatedclasses
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }


    }
    public class IndiaCity : City
    {
        [Display(Name = "State")]
        public IndiaState? indiastate { get; set; }
    }
}
