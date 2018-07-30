using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            // TODO:

            var fromAccount = this.accountRepository.GetAccountById(fromAccountId);

            var fromBalance = fromAccount.Balance - amount;
            if (fromBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

                       

            if (fromBalance < 500m)
            {
                this.notificationService.NotifyFundsLow(fromAccount.User.Email);
            }


            fromAccount.Balance -= amount;
            this.accountRepository.Update(fromAccount);
        }
    }
}
