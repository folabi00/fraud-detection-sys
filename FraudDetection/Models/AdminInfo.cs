namespace AdminAuth.Models
{
    public class AdminInfo
    {
        public static List<AdminModel> Admins = new List<AdminModel>()
        {
            new AdminModel() { UserName = "super_admin", Password = "admin@123", EmailAddress = "superadmin@gmail.com", Role = "Administrator"},
            new AdminModel() { UserName = "base_admin", Password = "admin@base", EmailAddress = "baseadmin@gmail.com", Role = "Administrator"} 
        };
    }
}
