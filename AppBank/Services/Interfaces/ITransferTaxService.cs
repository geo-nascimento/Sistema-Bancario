
namespace AppBank.Services.Interfaces
{
    public interface ITransferTaxService
    {
        public decimal TaxFee(decimal amount, decimal tax);
    }
}
