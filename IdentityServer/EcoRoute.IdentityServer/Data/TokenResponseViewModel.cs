using System;

namespace EcoRoute.IdentityServer.Data
{
    public class TokenResponseViewModel
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }

        public TokenResponseViewModel() { }

        public TokenResponseViewModel(string token, DateTime expireDate)
        {
            Token = token;
            ExpireDate = expireDate;
        }
    }
}
