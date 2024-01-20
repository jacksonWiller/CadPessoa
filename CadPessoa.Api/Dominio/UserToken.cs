using System;

namespace CadPessoa.Api.Dominio
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
