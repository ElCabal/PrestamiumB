using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Prestamium.Persistence
{
    public class PrestamiumUserIdentity : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; } = default!;

        [StringLength(50)]
        public string LastName { get; set; } = default!;
    }
}
