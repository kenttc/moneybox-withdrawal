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

            fromAccount.RunWithdrawalChecks(amount, this.notificationService);
          
            //fromAccount.Balance = fromAccount.Balance - amount;

            //fromAccount.Withdrawn = fromAccount.Withdrawn - amount;

            this.accountRepository.Update(fromAccount);
        }
    }
}
