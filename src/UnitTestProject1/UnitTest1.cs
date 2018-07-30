using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moneybox.App.Features;
using Moneybox.App.DataAccess;
using Moq;

using Moneybox.App;
using Moneybox.App.Domain.Services;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TransferMoney_Given_From_account_has_enough_money_and_to_account_has_0_will_remove_amount_withdrawn_from_fromAccountBalance_and_add_it_toToAccountBalance()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");
            var toAccountId = new System.Guid("065a008a-e33e-4576-8f62-fd1f306e3202");
            var toAccount = new Account();
            var fromAccount = new Account() { Balance=1000m,};


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);
            accountRepoMock.Setup(xx => xx.GetAccountById(toAccountId)).Returns(toAccount);

            

            var sut = new TransferMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, toAccountId, 100.0m);

            Assert.AreEqual(900m, fromAccount.Balance);
            Assert.AreEqual(100m, toAccount.Balance);
        }


        [TestMethod]
        public void WithdrawMoney_Given_From_account_has_enough_money_balance_will_be_reduced_by_the_amount_withdrawn()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");
            
            
            var fromAccount = new Account() { Balance = 1000m, };


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);
            


            var sut = new WithdrawMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, 100.0m);

            Assert.AreEqual(900m, fromAccount.Balance);
            
        }


        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException), "Insufficient funds to make transfer")]
        public void WithdrawMoney_execute_when_amount_withdrawn_results_in_negative_value_will_throw_exception()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");


            var fromAccount = new Account() { Balance = 1000m, };


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);



            var sut = new WithdrawMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, 1001.0m);

            

        }

        [TestMethod]
        public void WithdrawMoney_Given_From_account_balance_is_less_than_500m_after_withdrawal_fire_a_notification_NotifyFundsLow()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");

            var user = new User() { Email = "test@email.com" };
            var fromAccount = new Account() { Balance = 1000m,User = user };


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);



            var sut = new WithdrawMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, 600.0m);

            Assert.AreEqual(400m, fromAccount.Balance);

            notificationServiceMock.Verify(x => x.NotifyFundsLow(fromAccount.User.Email), Times.Once);

        }

        [TestMethod]
        public void WithdrawMoney_Ensure_that_account_repo_is_called_to_update_account()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");

            var user = new User() { Email = "test@email.com" };
            var fromAccount = new Account() { Balance = 1000m, User = user };


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);



            var sut = new WithdrawMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, 600.0m);

            Assert.AreEqual(400m, fromAccount.Balance);
            accountRepoMock.Verify(x => x.Update(It.IsAny<Account>()), Times.Once);
            

        }


        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException), "Insufficient funds to make transfer")]
        public void TransferMoney_execute_when_amount_withdrawn_results_in_negative_value_will_throw_exception()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");
            var toAccountId = new System.Guid("065a008a-e33e-4576-8f62-fd1f306e3202");
            var toAccount = new Account();
            var fromAccount = new Account() { Balance = 1000m, };


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);
            accountRepoMock.Setup(xx => xx.GetAccountById(toAccountId)).Returns(toAccount);



            var sut = new TransferMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, toAccountId, 1001.0m);

            
        }



        [TestMethod]
        public void TransferMoney_Given_From_account_balance_is_less_than_500m_after_withdrawal_fire_a_notification_NotifyFundsLow()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");
            var toAccountId = new System.Guid("065a008a-e33e-4576-8f62-fd1f306e3202");

            var user = new User() { Email = "test@email.com" };
            var fromAccount = new Account() { Balance = 1000m, User = user };
            var toAccount = new Account();


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);
            accountRepoMock.Setup(xx => xx.GetAccountById(toAccountId)).Returns(toAccount);


            var sut = new TransferMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, toAccountId, 600.0m);

            Assert.AreEqual(400m, fromAccount.Balance);

            notificationServiceMock.Verify(x => x.NotifyFundsLow(fromAccount.User.Email), Times.Once);

        }



        [TestMethod]
        public void WithdrawMoney_ensure_withdrawn_is_updated_with_correct_amount()
        {
            var fromAccountGuid = new System.Guid("adc1c2b0-bb71-4205-bf95-91bdbda67d75");

            var user = new User() { Email = "test@email.com" };
            var fromAccount = new Account() { Balance = 1000m, User = user };


            var accountRepoMock = new Mock<IAccountRepository>();
            var notificationServiceMock = new Mock<INotificationService>();

            accountRepoMock.Setup(xx => xx.GetAccountById(fromAccountGuid)).Returns(fromAccount);



            var sut = new WithdrawMoney(accountRepoMock.Object, notificationServiceMock.Object);
            sut.Execute(fromAccountGuid, 600.0m);


            Assert.AreEqual(600m, fromAccount.Withdrawn);



        }

    }
}
