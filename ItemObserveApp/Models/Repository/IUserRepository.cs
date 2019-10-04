using System;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public interface IUserRepository
    {
        Task<UserSetting> GetUserSettingAsync();
        Task PutUserSettingAsync(UserSetting target);
    }
}
