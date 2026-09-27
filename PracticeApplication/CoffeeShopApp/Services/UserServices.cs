using CoffeeShopApp.Models;
using CoffeeShopApp.Repository;

namespace CoffeeShopApp.Services
{
    internal class UserServices
    {
        private UserRepository _userRepository;

        public UserServices(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        internal bool AddUser(int userId, string userName, string password)
        {
            Users user = new Users
            {
                UserId = userId,
                UserName = userName,
                Password = password
            };
            return this._userRepository.AddUserData(user);
        }

        internal bool CheckUser(int userId, string password)
        {
            return this._userRepository.CheckUserExists(userId, password);
        }
    }
}