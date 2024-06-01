using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.User
{
    public class SortUserSpec : Specification<UserModel>
    {
        public SortUserSpec(string[] roles, bool isBlocked)
        {
            Roles = roles;
            IsBlocked = isBlocked;

            Query.Where(x =>Roles.Contains(x.Role));

            Query.Where(x => x.IsBlocked.Equals(IsBlocked));
        }

        public string[] Roles { get; set; }
        public bool IsBlocked { get; set; }
    }
}
