using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryGoodNewsPortal.Data.Entities
{
    [Table("RefreshToken")]
    public class RefreshToken : BaseEntities
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }

        public string? ReplaceByToken { get; set; }
        public string? ReasonOfRevoke { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsExpired && !IsRevoked;

    }
}
