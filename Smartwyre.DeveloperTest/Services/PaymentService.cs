using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Configuration;

namespace Smartwyre.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private IAccountDataStore _accountDataStore;
        public PaymentService(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("request object is null");
            }

            var result = new MakePaymentResult();
            Account account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
            if (account != null)
            {
                if (account.Balance < request.Amount || account.Status != AccountStatus.Live || !account.AllowedPaymentSchemes.ToString().Equals(request.PaymentScheme.ToString()))
                {
                    result.Success = false;
                }
                else
                {
                    account.Balance -= request.Amount;
                    _accountDataStore.UpdateAccount(account);
                    result.Success = true;
                }
            }
            else
            {
                result.Success = false;
            }
            return result;
        }
           
    }
}
