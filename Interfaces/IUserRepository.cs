using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface IUserRepository:IRepository<int,User>
    {

        public Task<User> GetUserByUsername(string username);

        Task<User> GetUserByEmail(string email);

    }
}
