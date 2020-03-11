namespace CasaDeShowAPI.Model.ViewModels
{
    public static class VendaExtensions
    {
        public static Venda ToVenda(this VendaRequest vendaRequest){
            Venda venda = new Venda();
            venda.EventoId = vendaRequest.EventoId;
            venda.Quantidade = vendaRequest.Quantidade;

            return venda;
        }

        public static VendaResponse ToVendaResponse(this Venda venda, string uriEvento, string uriUsuario){
            return new VendaResponse(venda, uriEvento, uriUsuario);
        }
    }
}