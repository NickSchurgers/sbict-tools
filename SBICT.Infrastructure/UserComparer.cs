using System;
using System.Collections.Generic;
using SBICT.Data;

namespace SBICT.Infrastructure
{
    public class UserComparer : EqualityComparer<User>
    {
        private readonly IEqualityComparer<Guid> _c = EqualityComparer<Guid>.Default;

        public override bool Equals(User x, User y)
        {
            return y != null && x != null && _c.Equals(x.Id, y.Id);
        }

        public override int GetHashCode(User obj)
        {
            unchecked
            {
                var hash = 17;
                if (obj == null) return hash * 23 + int.MinValue;
                hash = hash * 23 + obj.UserName.GetHashCode();
                hash = hash * 23 + obj.Id.GetHashCode();

                return hash;
            }
        }
    }
}