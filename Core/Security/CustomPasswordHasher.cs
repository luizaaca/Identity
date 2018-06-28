using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
    public class CustomPasswordHasher<TUser> : PasswordHasher<TUser> where TUser: class
    {
        public CustomPasswordHasher(IOptions<PasswordHasherOptions> optionsAccessor = null) : base(optionsAccessor)
        {
        }

        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) { throw new ArgumentNullException(nameof(hashedPassword)); }
            if (providedPassword == null) { throw new ArgumentNullException(nameof(providedPassword)); }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // read the format marker from the hashed password
            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failed;
            }

            if (hashedPassword == providedPassword)
                return PasswordVerificationResult.Success;

            // ASP.NET Core uses 0x00 and 0x01
            if (decodedHashedPassword[0] == 0x00 || decodedHashedPassword[0] == 0x01)
            {
                return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
            }

            return PasswordVerificationResult.Failed;
        }

        public override string HashPassword(TUser user, string password)
        {
            return password;
        }
    }
}
