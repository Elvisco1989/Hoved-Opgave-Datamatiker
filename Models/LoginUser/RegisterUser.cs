using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models.LoginUser
{
    public class RegisterUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation did not match")]
        public string ConfirmPassword { get; set; }

    }
}
