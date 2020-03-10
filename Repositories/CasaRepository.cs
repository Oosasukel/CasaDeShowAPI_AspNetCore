using System.Collections.Generic;
using System.Linq;
using CasaDeShowAPI.Data;
using CasaDeShowAPI.Model;
using CasaDeShowAPI.Model.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CasaDeShowAPI.Repositories
{
    public interface ICasaRepository
    {
        List<Casa> GetCasas();
        List<Casa> GetCasasAsc();
        List<Casa> GetCasasDesc();
        void Add(Casa casa);
        Casa GetCasa(string id);
        List<Evento> GetEventos(string id);
        Casa Update(Casa casa);
        void Delete(Casa casa);
        Casa GetCasaByNome(string nome);
        List<Casa> GetByEmail(string email);
    }

    public class CasaRepository : ICasaRepository
    {
        private readonly DataContext _contexto;

        public CasaRepository(DataContext contexto)
        {
            _contexto = contexto;
        }

        public void Add(Casa casa)
        {
            casa.Id = System.Guid.NewGuid().ToString();
            _contexto.Casas.Add(casa);
            _contexto.SaveChanges();
        }

        public void Delete(Casa casa)
        {
            casa = _contexto.Casas.Where(casaDb => casaDb.Id == casa.Id).SingleOrDefault();
            if(casa != null){
                casa.Status = CasaStatus.Excluida;
                var eventos = _contexto.Eventos.Where(evento => evento.CasaId == casa.Id && evento.Status != EventoStatus.Excluido).ToList();
                foreach (var evento in eventos)
                {
                    evento.Status = EventoStatus.Excluido;
                }
            }
            _contexto.SaveChanges();
        }

        public List<Casa> GetByEmail(string email)
        {
            return _contexto.Casas
                        .AsNoTracking()
                        .Where(casa => casa.UsuarioEmail == email && casa.Status != CasaStatus.Excluida)
                        .ToList();
        }

        public Casa GetCasa(string id)
        {
            return _contexto.Casas.AsNoTracking().Where(casa => casa.Id == id && casa.Status == CasaStatus.Ativa).SingleOrDefault();
        }

        public Casa GetCasaByNome(string nome)
        {
            return _contexto.Casas.AsNoTracking().Where(casa => casa.Nome == nome && casa.Status == CasaStatus.Ativa).SingleOrDefault();
        }

        public List<Casa> GetCasas()
        {
            return _contexto.Casas.AsNoTracking().Where(casa => casa.Status == CasaStatus.Ativa).ToList();
        }

        public List<Casa> GetCasasAsc()
        {
            return _contexto.Casas.AsNoTracking().Where(casa => casa.Status == CasaStatus.Ativa).OrderBy(casa => casa.Nome).ToList();
        }

        public List<Casa> GetCasasDesc()
        {
            return _contexto.Casas.AsNoTracking().Where(casa => casa.Status == CasaStatus.Ativa).OrderByDescending(casa => casa.Nome).ToList();
        }

        public List<Evento> GetEventos(string id)
        {
            return _contexto.Eventos.AsNoTracking().Where(evento => evento.Casa.Id == id && evento.Status == EventoStatus.Ativo).ToList();
        }

        public Casa Update(Casa casa)
        {
            _contexto.Update(casa);
            _contexto.SaveChanges();
            return casa;
        }
    }
}