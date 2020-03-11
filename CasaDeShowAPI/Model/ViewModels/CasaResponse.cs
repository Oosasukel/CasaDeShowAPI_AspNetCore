using Microsoft.AspNetCore.Mvc;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class CasaResponse
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Eventos { get; set; }
        
        public CasaResponse(Casa casa, string eventosUri)
        {
            this.Id = casa.Id;
            this.Nome = casa.Nome;
            this.Endereco = casa.Endereco;
            this.Eventos = eventosUri;
        }
    }
}