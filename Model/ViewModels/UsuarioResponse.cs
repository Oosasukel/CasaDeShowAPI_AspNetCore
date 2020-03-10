namespace CasaDeShowAPI.Model.ViewModels
{
    public class UsuarioResponse
    {
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Compras { get; set; }

        public UsuarioResponse(Usuario usuario, string comprasUri){
            Email = usuario.Email;
            Nome = usuario.Nome;
            Compras = comprasUri;
        }
    }
}