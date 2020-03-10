namespace CasaDeShowAPI.Model.ViewModels
{
    public static class UsuarioExtensions
    {
        public static UsuarioResponse ToUsuarioResponse(this Usuario usuario, string comprasUri){
            return new UsuarioResponse(usuario, comprasUri);
        }

        public static Usuario ToUsuario(this UsuarioRequest usuarioRequest){
            var usuario = new Usuario();
            usuario.Nome = usuarioRequest.Nome;
            usuario.Senha = usuarioRequest.Senha;
            usuario.Email = usuarioRequest.Email;
            return usuario;
        }
    }
}