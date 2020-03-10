using System.Collections.Generic;
using System.Linq;
using CasaDeShowAPI.Data;
using CasaDeShowAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CasaDeShowAPI.Repositories
{
    public interface IEventoRepository
    {
        Evento GetEvento(string id);
        List<Evento> GetEventos();
        bool Add(Evento evento, string email);
        void Delete(string eventoId);
        List<Evento> GetEventosCapacidadeAsc();
        List<Evento> GetEventosCapacidadeDesc();
        List<Evento> GetEventosDataAsc();
        List<Evento> GetEventosDataDesc();
        List<Evento> GetEventosNomeAsc();
        List<Evento> GetEventosNomeDesc();
        List<Evento> GetEventosPrecoAsc();
        List<Evento> GetEventosPrecoDesc();
        bool Update(Evento evento);
        List<Venda> GetVendas(string eventoId);
        List<Evento> GetByEmail(string email);
    }

    public class EventoRepository : IEventoRepository
    {
        private readonly DataContext _contexto;

        public EventoRepository(DataContext contexto)
        {
            _contexto = contexto;
        }

        public List<Evento> GetEventos()
        {
            return _contexto.Eventos.AsNoTracking().Where(evento => evento.Status == EventoStatus.Ativo).ToList();
        }

        public Evento GetEvento(string id)
        {
            return _contexto.Eventos.Include(evento => evento.Casa).AsNoTracking().Where(evento => evento.Id == id).SingleOrDefault();
        }

        public bool Add(Evento evento, string email)
        {
            var casa = _contexto.Casas.AsNoTracking().Where(casa => casa.Id == evento.CasaId).SingleOrDefault();
            if(casa == null) return false;

            if(casa.UsuarioEmail != email) return false;

            evento.Id = System.Guid.NewGuid().ToString();
            _contexto.Eventos.Add(evento);
            _contexto.SaveChanges();
            return true;
        }

        public void Delete(string eventoId)
        {
            var evento = _contexto.Eventos.Where(e => e.Id == eventoId).SingleOrDefault();
            if(evento != null){
                evento.Status = EventoStatus.Excluido;
                _contexto.SaveChanges();
            }
        }

        public List<Evento> GetEventosCapacidadeAsc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderBy(evento => evento.Capacidade).
                ToList();
        }

        public List<Evento> GetEventosCapacidadeDesc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderByDescending(evento => evento.Capacidade).
                ToList();
        }

        public List<Evento> GetEventosDataAsc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderBy(evento => evento.Data).
                ToList();
        }

        public List<Evento> GetEventosDataDesc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderByDescending(evento => evento.Data).
                ToList();
        }

        public List<Evento> GetEventosNomeAsc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderBy(evento => evento.Nome).
                ToList();
        }

        public List<Evento> GetEventosNomeDesc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderByDescending(evento => evento.Nome).
                ToList();
        }

        public List<Evento> GetEventosPrecoAsc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderBy(evento => evento.Preco).
                ToList();
        }

        public List<Evento> GetEventosPrecoDesc()
        {
            return _contexto.Eventos.
                AsNoTracking().
                Where(evento => evento.Status == EventoStatus.Ativo).
                OrderByDescending(evento => evento.Preco).
                ToList();
        }

        public bool Update(Evento evento)
        {
            Casa casa = _contexto.Casas.AsNoTracking().Where(casa => casa.Id == evento.CasaId).SingleOrDefault();
            if(casa == null) return false;
            
            _contexto.Eventos.Update(evento);
            _contexto.SaveChanges();
            return true;
        }

        public List<Venda> GetVendas(string eventoId)
        {
            return _contexto.Vendas.AsNoTracking().Where(venda => venda.EventoId == eventoId && venda.Status != VendaStatus.Cancelada).ToList();
        }

        public List<Evento> GetByEmail(string email)
        {
            return _contexto.Eventos
                    .AsNoTracking()
                    .Include(evento => evento.Casa)
                    .Where(e => e.Status != EventoStatus.Excluido && e.Casa.UsuarioEmail == email)
                    .ToList();
        }
    }
}