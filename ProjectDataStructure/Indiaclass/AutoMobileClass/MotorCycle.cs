using ProjectDataStructure.Enum;

namespace ProjectDataStructure.Indiaclass.AutoMobile
{
    public class MotorCycle
    {
        public int Id { get; set; }
        public MotorCycleBrand Make { get; set; }
        public ModelYear Modelyear { get; set; }
        public string Description { get; set; }
        public string DemagePart { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public double Prize { get; set; }
    }
}
