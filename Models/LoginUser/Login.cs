using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models.LoginUser
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
