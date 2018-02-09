using Common.Interfaces;

namespace Common.Models
{
    public class LoginOptionsRequest : IValidate
    {
        public string Login { set; get; }
        public string Password { set; get; }
        public bool IsValid => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
    }
}
