namespace AdminAuth.Models
{
    public class AdminInfo
    {
        public static List<Admin> Admins = new List<Admin>()
        {
            new Admin() { UserName = "super_admin", Password = "admin@123", EmailAddress = "superadmin@gmail.com", Role = "Administrator"},
            new Admin() { UserName = "base_admin", Password = "admin@base", EmailAddress = "baseadmin@gmail.com", Role = "Administrator"} 
        };
    }
}
