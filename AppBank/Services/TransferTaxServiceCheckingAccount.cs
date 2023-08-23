using AppBank.Services.Interfaces;

namespace AppBank.Services
{
    internal class TransferTaxServiceCheckingAccount : ITransferTaxService
    {
        public decimal TaxFee(decimal amount, decimal tax)
        {
            decimal taxedValue = amount * (1 - tax / 100.0M);

            return taxedValue;
        }
    }
}
