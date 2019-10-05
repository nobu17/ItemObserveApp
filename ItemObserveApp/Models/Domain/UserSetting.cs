using System;
namespace ItemObserveApp.Models.Domain
{
    public class UserSetting
    {
        public UserSetting()
        {
            UserID = "";
            Password = "";
            Token = "";
        }

        public string UserID { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
