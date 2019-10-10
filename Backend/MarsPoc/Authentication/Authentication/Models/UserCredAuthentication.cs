using Authentication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class UserCredAuthentication : IUserCredAuthentication
    {
        private AuthDbContext context;
        public UserCredAuthentication(AuthDbContext context)
        {
            this.context = context;
        }
        public async Task<AuthDbModel> Authenticate(string userName, string Password)
        {
           AuthDbModel user = null;

            try
            {
                await Task.Run(() =>
                    { 
                        var users = from c in this.context.Users
                                   where c.UserName == userName && c.Password.Equals(Password)
                                   select c;

                        if (users != null)
                            user = users.FirstOrDefault();

                    });
            }

            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return user;
        }
    }
}
