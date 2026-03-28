using ProjectDataStructure.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProjectDataStructure.CustomValidators
{
    public class Validation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<ServicesTypesViewModel> instance = value as List<ServicesTypesViewModel>;
            int count = instance == null ? 0 : (from p in instance
                                                where p.isSelected == true
                                                select p).Count();
            if (count >= 1)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
