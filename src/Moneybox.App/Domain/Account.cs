using System;

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

        public void CheckBalance(decimal amountWithdrawing)
        {
            if (this.Balance - amountWithdrawing < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
        }

    }
}
