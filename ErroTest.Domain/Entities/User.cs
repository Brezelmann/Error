using System;
using System.Collections.Generic;
using ErrorTest.Domain.ValueObjects;

namespace ErrorTest.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public FullName Name { get; set; }

        public virtual ICollection<UserClaims> Claims { get; set; }
    }
}