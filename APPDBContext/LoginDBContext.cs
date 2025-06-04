using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.APPDBContext
{
    public class LoginDBContext: IdentityDbContext<IdentityUser>
    {
        public LoginDBContext()
        {

        }

        public LoginDBContext(DbContextOptions<LoginDBContext> options) : base(options)
        {

        }
    }
}
