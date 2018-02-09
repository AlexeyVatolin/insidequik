using Microsoft.AspNet.Identity;

namespace ServerCore.Base.IdentityConfig
{
    class PasswordHasherThatDoingNothing : PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return password;
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword.Equals(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
