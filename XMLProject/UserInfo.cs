using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLProject
{
    internal class UserInfo
    {
        public static string GetUserName()
        {
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    var user = UserPrincipal.Current;
                    return user?.DisplayName ?? Environment.UserName; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user name: {ex.Message}");
                return Environment.UserName;
            }
        }
    }
}
