namespace CoffeeShopApp.Models
{
    internal class Users
    {
        public Users()
        {
        }

        public Users(int UserId, string UserName, string Password)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.Password = Password;
        }

        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
