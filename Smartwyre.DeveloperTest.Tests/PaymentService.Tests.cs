using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        [Fact]
        public void TestMakePaymentSuccessfully()
        {
            //Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            IPaymentService paymentService = new PaymentService(mockAccountDataStore.Object);
            Account account = new Account { AccountNumber = "123123232323", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer, Status = AccountStatus.Live };
            mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            bool expectedResult = true;

            var paymentRequest = new MakePaymentRequest
            {
                Amount = 200,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.BankToBankTransfer,
                DebtorAccountNumber = "123123232323"
            };

            //Act
            var actualResult = paymentService.MakePayment(paymentRequest);

            //Assert
            Assert.True(expectedResult == actualResult.Success);
        }

        [Fact]
        public void TestMakePaymentFailedAccountNotLive()
        {
            //Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            IPaymentService paymentService = new PaymentService(mockAccountDataStore.Object);
            Account account = new Account
            {
                AccountNumber = "123123232323",
                Balance = 1000,
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer,
                Status = AccountStatus.Disabled
            };
            mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            bool expectedResult = false;

            var paymentRequest = new MakePaymentRequest
            {
                Amount = 200,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.BankToBankTransfer,
                DebtorAccountNumber = "123123232323"
            };

            //Act
            var actualResult = paymentService.MakePayment(paymentRequest);

            //Assert
            Assert.True(expectedResult == actualResult.Success);
        }

        [Fact]
        public void TestMakePaymentFailedAccountPaymentSchemeMisMatch()
        {
            //Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            IPaymentService paymentService = new PaymentService(mockAccountDataStore.Object);
            Account account = new Account
            {
                AccountNumber = "123123232323",
                Balance = 1000,
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer,
                Status = AccountStatus.Live
            };
            mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            bool expectedResult = false;

            var paymentRequest = new MakePaymentRequest
            {
                Amount = 200,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.AutomatedPaymentSystem,
                DebtorAccountNumber = "123123232323"
            };

            //Act
            var actualResult = paymentService.MakePayment(paymentRequest);

            //Assert
            Assert.True(expectedResult == actualResult.Success);
        }

        [Fact]
        public void TestMakePaymentFailedAccountBalanceNotEnough()
        {
            //Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            IPaymentService paymentService = new PaymentService(mockAccountDataStore.Object);
            Account account = new Account
            {
                AccountNumber = "123123232323",
                Balance = 100,
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer,
                Status = AccountStatus.Live
            };
            mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            bool expectedResult = false;

            var paymentRequest = new MakePaymentRequest
            {
                Amount = 200,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.AutomatedPaymentSystem,
                DebtorAccountNumber = "123123232323"
            };

            //Act
            var actualResult = paymentService.MakePayment(paymentRequest);

            //Assert
            Assert.True(expectedResult == actualResult.Success);
        }
    }
}
