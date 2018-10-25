using System;

namespace ErrorTest.Domain.Entities
{
    public class UserClaims
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}