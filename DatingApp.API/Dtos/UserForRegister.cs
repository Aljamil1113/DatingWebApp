using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegister
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage="You must specify password length between 4 and 8")]
        public string Password { get; set; }
    }
}