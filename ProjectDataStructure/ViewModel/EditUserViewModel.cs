using ProjectDataStructure.Addressrelatedclasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectDataStructure.ViewModel
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IndiaUserAddress UserAddress { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
