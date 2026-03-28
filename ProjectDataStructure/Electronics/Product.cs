using ProjectDataStructure.IdentityClass;

namespace ProjectDataStructure.Electronics
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrize { get; set; }
        public string PhotoPath { get; set; }
        public ApplicationUser applicationUser { get; set; }
    }
}
