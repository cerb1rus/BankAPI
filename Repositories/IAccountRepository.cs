using bankApi.Models;

namespace bankApi.Repositories
{
    public interface IAccountRepository
    {
        List<Account> GetAll();
        Account? GetById(Guid id);
        List<Account> GetByOwnerId(Guid ownerId);
        Account Create(Account account);
        Account Update(Account account);
        Account Replace(Account account);
        bool Delete(Guid id);
        List<Transaction> GetTransactions(Guid accountId);
        void AddTransaction(Guid accountId, Transaction transaction);
        Account? GetAccountWithTransactions(Guid id);
        void UpdateBalance(Guid accountId, decimal newBalance);
    }
}