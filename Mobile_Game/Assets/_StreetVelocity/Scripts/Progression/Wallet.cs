using System;

namespace StreetVelocity.Progression
{
    public sealed class Wallet
    {
        public int Credits { get; private set; }
        public event Action<int> BalanceChanged;
        public Wallet(int initialCredits = 0) { Credits = Math.Max(0, initialCredits); }
        public bool TrySpend(int amount)
        {
            if (amount < 0 || Credits < amount) return false;
            Credits -= amount; BalanceChanged?.Invoke(Credits); return true;
        }
        public void Add(int amount)
        {
            if (amount <= 0) return;
            Credits = Credits > int.MaxValue - amount ? int.MaxValue : Credits + amount;
            BalanceChanged?.Invoke(Credits);
        }
    }
}
