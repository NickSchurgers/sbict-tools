using System;
using System.Collections.Generic;
using SBICT.Data;

namespace SBICT.Infrastructure
{
    public class UserComparer : IEqualityComparer<IUser>
    {
        private readonly IEqualityComparer<Guid> _c = EqualityComparer<Guid>.Default;

        public bool Equals(IUser x, IUser y)
        {
            return  _c.Equals(x.Id, y.Id);
        }

        public int GetHashCode(IUser obj)
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + obj.UserName.GetHashCode();
                hash = hash * 23 + obj.Id.GetHashCode();

                return hash;
            }
        }
    }
}