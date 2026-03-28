using ProjectDataStructure.Enum;
using ProjectDataStructure.Indiaclass.AutoMobile;
using System.ComponentModel;

namespace ProjectDataStructure.BaseClasses
{
    public class Car
    {
        public int Id { get; set; }
        public ModelYear ModelYear { get; set; }
        public string Description { get; set; }
        [DisplayName("AnyDamage")]
        public string DemagePart { get; set; }
        public string PhotoPath { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public Country Country { get; set; }
        public double Prize { get; set; }
    }
}
