using Microsoft.AspNetCore.Http;
using ProjectDataStructure.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDataStructure.IndiaViewModel
{
    public class ServiceproviderViewModel
    {
        public int serviceproviderId { get; set; }
        public string ApplicationUserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        //[Required]
        //[EmailAddress]
        //[Remote(action: "IsEmailInUse", controller: "Electronics", ErrorMessage = "Do login And Try To Save Inquiry")]
        public string Email { get; set; }
        [Required]
        [MaxLength(40)]
        [Display(Name = "KindOfServices")]
        public string Service { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [MaxLength(10, ErrorMessage = "Contact Number Should Be 10 Digit")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string phoneNumber { get; set; }
        [Required]
        public string BusinessAddress { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        [MaxLength(6, ErrorMessage = "Zipcode Number Should Be 6 Digit")]
        [RegularExpression(@"\d{6}$", ErrorMessage = "Invalid Zip Code")]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Display(Name = "State")]
        public IndiaState? IndiaState { get; set; }
        [Display(Name ="State")]
        public int stateID { get; set; }
        public int? AddressId { get; set; }
        public IFormFile BusinessPhoto { get; set; }
        //[Validation(ErrorMessage = "Select at least 1 Service")]
        public List<ServicesTypesViewModel> ServicesTypes { get; set; }
        public string photopath { get; set; }
        [Required]
        public string BusinessName { get; set; }


    }

}
