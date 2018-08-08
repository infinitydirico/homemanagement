using System;

namespace HomeManagement.Domain
{
    public class Token
    {
        public DateTime IssueDate { get; set; }

        public string Value { get; set; }

        public static Token GenerateToken()
        {
            var token = new Token
            {
                IssueDate = DateTime.Now,
                Value = Guid.NewGuid().ToString("N")
            };

            return token;
        }

    }
}