using System;
namespace ItemObserveApp.Models.Domain
{
    public class LoginResult
    {
        public LoginResult()
        {
            IsSuccessed = false;
        }

        public bool IsSuccessed { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
    }
}
