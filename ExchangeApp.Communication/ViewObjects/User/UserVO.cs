namespace ExchangeApp.Communication.ViewObjects.User
{
    public class UserVO
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Whatsapp { get; set; }
        public string Instagram { get; set; }
        public List<string> RolesName { get; set; }
    }
}
