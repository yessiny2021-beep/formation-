using Microsoft.AspNetCore.Identity;

namespace MvcMovie.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Optional link to Employe
        public int? EmployeId { get; set; }
        public Employe? Employe { get; set; }
    }
}