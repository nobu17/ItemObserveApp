using System;
using System.Threading.Tasks;
using ItemObserveApp.Models.Domain;

namespace ItemObserveApp.Models.Repository
{
    public interface ILoginRepository
    {
        Task<LoginResult> LoginAsync(string userID, string password);
    }
}
