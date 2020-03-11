using System.Collections.Generic;
using System.Linq;
using CasaDeShowAPI.Data;
using CasaDeShowAPI.Model;
using CasaDeShowAPI.Model.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CasaDeShowAPI.Repositories
{
    public interface IUsuarioRepository
    {
        List<Usuario> Get();
        Usuario Get(string email);
        void Add(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(Usuario usuario);
        bool IsValid(Login login);
        List<Venda> GetCompras(string email);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _contexto;

        public UsuarioRepository(DataContext contexto)
        {
            _contexto = contexto;
        }

        public void Add(Usuario usuario)
        {
            _contexto.Usuarios.Add(usuario);
            _contexto.SaveChanges();
        }

        public void Delete(Usuario usuario)
        {
            usuario = _contexto.Usuarios.Where(u => u.Email == usuario.Email).SingleOrDefault();
            if(usuario != null){
                usuario.Status = UsuarioStatus.Excluido;
                _contexto.SaveChanges();
            }
        }

        public List<Usuario> Get()
        {
            return _contexto.Usuarios.AsNoTracking().Where(usuario => usuario.Status == UsuarioStatus.Ativo).ToList();
        }

        public Usuario Get(string email)
        {
            return _contexto.Usuarios.AsNoTracking().Where(usuario => usuario.Email == email && usuario.Status == UsuarioStatus.Ativo).SingleOrDefault();
        }

        public List<Venda> GetCompras(string email)
        {
            return _contexto.Vendas
                        .AsNoTracking()
                        .Where(venda => venda.UsuarioEmail == email && venda.Status != VendaStatus.Cancelada)
                        .ToList();
        }

        public bool IsValid(Login login)
        {
            var usuario = _contexto.Usuarios.AsNoTracking().Where(u => u.Email == login.Email && u.Senha == login.Senha).SingleOrDefault();
            if(usuario == null) return false;
            else return true;
        }

        public void Update(Usuario usuario)
        {
            _contexto.Usuarios.Update(usuario);
            _contexto.SaveChanges();
        }
    }
}