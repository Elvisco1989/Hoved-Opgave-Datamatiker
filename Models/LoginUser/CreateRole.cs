using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models.LoginUser
{
    public class CreateRole
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

    }
}
