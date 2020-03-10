using System.Collections.Generic;

namespace CasaDeShowAPI.Model
{
    public enum UsuarioStatus{Ativo, Excluido}
    public class Usuario
    {
        public string Email { get; set; }
        public string Nome { get; set; }
        public List<Venda> Compras { get; set; }
        public string Senha { get; set; }

        public UsuarioStatus Status{get; set;}
        public string Role { get; set; }

        public Usuario(){
            Status = UsuarioStatus.Ativo;
        }
    }
}