// <copyright file="UserComparer.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using SBICT.Data;

    /// <inheritdoc />¶
    public class UserComparer : IEqualityComparer<IUser>
    {
        private readonly IEqualityComparer<Guid> c = EqualityComparer<Guid>.Default;

        /// <inheritdoc />
        public bool Equals(IUser x, IUser y)
        {
            return y != null && x != null && this.c.Equals(x.Id, y.Id);
        }

        /// <inheritdoc />
        public int GetHashCode(IUser obj)
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 23) + obj.UserName.GetHashCode();
                hash = (hash * 23) + obj.Id.GetHashCode();

                return hash;
            }
        }
    }
}