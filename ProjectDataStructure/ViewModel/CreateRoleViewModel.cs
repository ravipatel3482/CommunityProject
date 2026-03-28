using System.ComponentModel.DataAnnotations;

namespace ProjectDataStructure.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
