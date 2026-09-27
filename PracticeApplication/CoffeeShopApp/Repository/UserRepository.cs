using System.Text.Json;
using System.Text.Json.Serialization;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Repository
{
    internal class UserRepository
    {
        private readonly List<Users> _users;
        private readonly string _filePath;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(),
            }
        };

        public UserRepository(string filePath)
        {
            this._filePath = filePath;
            this._users = LoadAll();
        }

        internal bool AddUserData(Users user)
        {
            this._users.Add(user);
            this.WriteAll();
            return true;
        }

        internal bool CheckUserExists(int userId, string password)
        {
            return this._users.Any(user => user.UserId == userId && user.Password == password);
        }

        private void WriteAll()
        {
            string fileData = JsonSerializer.Serialize(this._users, this._jsonSerializerOptions);
            File.WriteAllText(this._filePath, fileData);
        }

        private List<Users> LoadAll()
        {
            if (!File.Exists(this._filePath))
            {
                return new List<Users>();
            }
            string fileData = File.ReadAllText(this._filePath);
            return JsonSerializer.Deserialize<List<Users>>(fileData, this._jsonSerializerOptions) ?? new List<Users>();
        }

    }
}
