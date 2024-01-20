using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace CadPessoa.Api.Dominio
{
    public class PessoaFisica
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Image { get; set; }
    }
}
