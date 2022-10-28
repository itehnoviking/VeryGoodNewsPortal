using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryGoodNewsPortal.Data.Entities
{
    public class User : BaseEntities
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
