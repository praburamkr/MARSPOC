using Authentication.Controllers;
using Common.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class UserAuthentication : IUserAuthentication
    {
        private IUserCredAuthentication userCredentialsAuth;
        private IJWTToken jwtToken;
        private IOAuthToken oauthToken;
        private AuthDbContext context;
        private IConfiguration configuration;

        public UserAuthentication(IUserCredAuthentication userCredentialsAuth,
                                  IJWTToken jwtToken,
                                  IOAuthToken oauthToken,
                                  AuthDbContext context,
                                  IConfiguration configuration)
        {
            this.userCredentialsAuth = userCredentialsAuth;
            this.oauthToken = oauthToken;
            this.jwtToken = jwtToken;
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<MarsResponse> SignIn(string userName, string password)
        {
                //return await Task.Run(() =>
                //{
                    var resp = new MarsResponse();
                    resp.Code = HttpStatusCode.ServiceUnavailable;
                    try
                {

                    

                    var tokenConfig = configuration.GetSection("TokenScheme").GetSection("Token").Value;

                    var tokenResp = await oauthToken.GetToken(userName, password);

                    if (tokenResp == null || string.IsNullOrEmpty(tokenResp.access_token))
                    {
                        resp.Code = HttpStatusCode.Unauthorized;
                        resp.Error = new MarsException("Unautherized access");
                    }
                    else
                    {
                        resp.Data = tokenResp;
                        resp.Code = HttpStatusCode.OK;
                    }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }

                    return resp;
                //});
        }

        public async Task<string> TokenAuthentication(string token)
        {
            string tokenString = null;

            try
            {
                await Task.Run(() =>
                {
                    var user = this.context.Users.FirstOrDefault(x => x.Token == token);

                    if (user != null)
                        tokenString = user.Token;
                    else
                    {
                        Console.WriteLine("Token is not found");
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return tokenString;
        }
    }
}