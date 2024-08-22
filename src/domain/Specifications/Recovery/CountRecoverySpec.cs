using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Recovery
{
    public class CountRecoverySpec : Specification<RecoveryModel>
    {
        public CountRecoverySpec()
        {
            if (UserId.HasValue)
                Query.Where(x => x.UserId.Equals(UserId.Value));

            if (IsExpired.HasValue)
                Query.Where(x => x.IsExpired.Equals(IsExpired));

            if (IsUsed.HasValue)
                Query.Where(x => x.IsUsed.Equals(IsUsed));
        }

        public bool? IsUsed { get; set; }
        public bool? IsExpired { get; set; }
        public int? UserId { get; set; }
    }
}
