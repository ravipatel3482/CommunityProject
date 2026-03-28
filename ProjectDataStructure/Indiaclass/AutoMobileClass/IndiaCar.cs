using ProjectDataStructure.Addressrelatedclasses;
using ProjectDataStructure.BaseClasses;
using ProjectDataStructure.Enum;

namespace ProjectDataStructure.Indiaclass.AutoMobile
{
    public class IndiaCar : Car
    {

        public IndiaAutoMaker Make { get; set; }
        public string User { get; set; }
        public string Email { get; set; }
        public IndiaCity Indiacity { get; set; }
    }
}
