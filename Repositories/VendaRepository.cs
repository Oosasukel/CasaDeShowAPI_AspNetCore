using System.Collections.Generic;
using System.Linq;
using CasaDeShowAPI.Data;
using CasaDeShowAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CasaDeShowAPI.Repositories
{
    public interface IVendaRepository
    {
        Venda Get(string id);
        List<Venda> Get();
        bool Add(Venda venda);
        void Cancelar(Venda venda);
        List<Venda> GetCompras(string email);
    }

    public class VendaRepository : IVendaRepository
    {
        private readonly DataContext _contexto;

        public VendaRepository(DataContext contexto)
        {
            _contexto = contexto;
        }

        public bool Add(Venda venda)
        {
            var evento = _contexto.Eventos.AsNoTracking().Where(evento => evento.Id == venda.EventoId).SingleOrDefault();
            if(evento == null) return false;

            var usuario = _contexto.Usuarios.AsNoTracking().Where(user => user.Email == venda.UsuarioEmail).SingleOrDefault();
            if(usuario == null) return false;

            venda.Id = System.Guid.NewGuid().ToString();

            venda.PrecoUnitario = evento.Preco;
            _contexto.Vendas.Add(venda);
            _contexto.SaveChanges();

            return true;
        }

        public void Cancelar(Venda venda)
        {
            venda.Status = VendaStatus.Cancelada;
            _contexto.Vendas.Update(venda);
            _contexto.SaveChanges();
        }

        public List<Venda> Get()
        {
            return _contexto.Vendas.AsNoTracking().Where(venda => venda.Status != VendaStatus.Cancelada).ToList();
        }

        public Venda Get(string id)
        {
            return _contexto.Vendas
                        .AsNoTracking()
                        .Where(venda => venda.Id == id)
                        .SingleOrDefault();
        }

        public List<Venda> GetCompras(string email)
        {
            return _contexto.Vendas
                        .AsNoTracking()
                        .Where(venda => venda.Status != VendaStatus.Cancelada && venda.UsuarioEmail == email)
                        .ToList();
        }
    }
}