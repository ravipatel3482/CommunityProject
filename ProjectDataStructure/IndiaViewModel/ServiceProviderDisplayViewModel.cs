using ProjectDataStructure.Enum;

namespace ProjectDataStructure.IndiaViewModel
{
    public class ServiceProviderDisplayViewModel
    {
        public int ProviderId { get; set; }
        public int AddressId { get; set; }
        public string ServiceType { get; set; }
        public string PhotoPath { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnerName { get; set; }
        public string ServicesProvided { get; set; }
        public IndiaState State { get; set; }
    }
}
