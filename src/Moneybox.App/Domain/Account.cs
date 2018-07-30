using System;
using Moneybox.App.Domain.Services;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public void RunWithdrawalChecks(decimal amountWithdrawing, INotificationService notificationService)
        {
            //usually i wouldn't put notification service in here but i'm not sure of how to do non-pocos so i'm trying to move stuff which are common into the same place do i can keep it DRY.

            if (this.Balance - amountWithdrawing < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
            if(this.Balance- amountWithdrawing < 500m)
            {
                notificationService.NotifyFundsLow(this.User.Email);
            }

        }

        

        


    }
}
