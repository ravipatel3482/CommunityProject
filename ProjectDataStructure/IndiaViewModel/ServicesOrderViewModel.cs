using ProjectDataStructure.Addressrelatedclasses;
using ProjectDataStructure.Enum;
using System.ComponentModel.DataAnnotations;


namespace ProjectDataStructure.IndiaViewModel
{
    public class ServicesOrderViewModel
    {
        [Key]
        public int ServicesInquiryID { get; set; }
        [Required]
        public string Name { get; set; }
        //[Required]
        //[EmailAddress]
        //[Remote(action: "IsEmailInUse", controller: "Electronics", ErrorMessage = "Do login And Try To Save Inquiry")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [MaxLength(10, ErrorMessage = "Contact Number Should Be 10 Digit")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNo { get; set; }
        [Required]
        public string ServicesAddress { get; set; }
        public IndiaCity City { get; set; }
        public string CityName { get; set; }
        [Required(ErrorMessage = "City Required")]
        public int CityId { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        [MaxLength(6, ErrorMessage = "Zipcode Number Should Be 6 Digit")]
        [RegularExpression(@"\d{6}$", ErrorMessage = "Invalid Zip Code")]
        public string Zipcode { get; set; }
        [Required]
        public string ServicesDescription { get; set; }
        public string ApplicationUserId { get; set; }
        public int? Servicesstatus { get; set; }
        public Servicesstatus? status { get; set; }
        public int? AddressId { get; set; }

       
    }
}
