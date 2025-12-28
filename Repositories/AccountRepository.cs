using bankApi.Models;

namespace bankApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = new();
        private readonly List<Transaction> _allTransactions = new();

        public List<Account> GetAll() => _accounts;

        public Account? GetById(Guid id) =>
            _accounts.FirstOrDefault(a => a.Id == id);

        public List<Account> GetByOwnerId(Guid ownerId) =>
            _accounts.Where(a => a.OwnerID == ownerId).ToList();

        public Account Create(Account account)
        {
            _accounts.Add(account);
            return account;
        }

        public Account Update(Account account)
        {
            var existing = GetById(account.Id);
            if (existing == null)
                throw new InvalidOperationException($"Account with id {account.Id} not found");

            existing.Rate = account.Rate;
            existing.DateCl = account.DateCl;

            return existing;
        }

        public Account Replace(Account account)
        {
            var existing = GetById(account.Id);
            if (existing == null)
                throw new InvalidOperationException($"Account with id {account.Id} not found");

            existing.OwnerID = account.OwnerID;
            existing.Type = account.Type;
            existing.CType = account.CType;
            existing.Balance = account.Balance;
            existing.Rate = account.Rate;
            existing.DateCl = account.DateCl;

            return existing;
        }

        public bool Delete(Guid id)
        {
            var account = GetById(id);
            if (account == null) return false;

            return _accounts.Remove(account);
        }

        public List<Transaction> GetTransactions(Guid accountId)
        {
            var account = GetById(accountId);
            return account?.Transactions ?? new List<Transaction>();
        }

        public void AddTransaction(Guid accountId, Transaction transaction)
        {
            var account = GetById(accountId);
            if (account != null)
            {
                account.Transactions.Add(transaction);
                _allTransactions.Add(transaction);
            }
        }

        public Account? GetAccountWithTransactions(Guid id) => GetById(id);

        public void UpdateBalance(Guid accountId, decimal newBalance)
        {
            var account = GetById(accountId);
            if (account != null)
            {
                account.Balance = newBalance;
            }
        }
    }
}