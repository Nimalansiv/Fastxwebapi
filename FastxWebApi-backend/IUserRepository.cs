<<<<<<< HEAD:FastxWebApi-backend/IUserRepository.cs
﻿using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface IUserRepository:IRepository<int,User>
    {
        public Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
    }
}
=======
﻿using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface IUserRepository:IRepository<int,User>
    {
        public Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Interfaces/IUserRepository.cs
