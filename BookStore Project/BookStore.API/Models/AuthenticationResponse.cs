using System.Collections.Generic;

namespace BookStore.API.Models
{
    public class AuthenticationResponse
    {
        public string Message { get; set; }
        public bool IsAuthentication { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
