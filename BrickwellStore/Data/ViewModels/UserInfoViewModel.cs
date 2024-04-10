namespace BrickwellStore.Data.ViewModels
{
    public class UserInfoViewModel
    {
        public string UserId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public List<string> Roles { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
