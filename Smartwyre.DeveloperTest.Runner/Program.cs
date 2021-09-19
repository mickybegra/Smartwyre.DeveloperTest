using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();           
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IAccountDataStore, AccountDataStore>();
            var sp = services.BuildServiceProvider();
            var paymentService = sp.GetRequiredService<IPaymentService>();
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 200,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.AutomatedPaymentSystem,
                DebtorAccountNumber = "233446765577"
            };
            var response = paymentService.MakePayment(request);
        
        }
    }
  
}
