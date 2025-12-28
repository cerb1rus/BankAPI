using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using bankApi.Models;
using Microsoft.AspNetCore.Mvc;
using bankApi.Repositories;
namespace bankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        public AccountsController(IAccountRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetAccountById(Guid id)
        {
            var Account = _repository.GetById(id);
            if (Account == null) return NotFound();
            return Ok(Account);
        }

        [HttpGet("Owner/{OwnerId}")]
        public IActionResult GetAccountsByOwnerId(Guid OwnerId)
        {
            var Accounts = _repository.GetByOwnerId(OwnerId);
            if (Accounts.Count == 0) return NotFound();
            return Ok(Accounts);
        }

        [HttpPost]
        public IActionResult AccountCreate([FromBody] AccountCreateRequest request)
        {
            if (request.Type == AccountType.Checking && request.Rate.HasValue)
                return BadRequest("Обычный счёт не может иметь процентную ставку");
            if (request.Type == AccountType.Checking && request.DateCl.HasValue)
                return BadRequest("Обычный счет не может иметь дату закрытия");
            if (request.Type == AccountType.Deposit && !request.Rate.HasValue)
                return BadRequest("Вклад обязан иметь процентную ставку");
            if (request.Type == AccountType.Deposit && !request.DateCl.HasValue)
                return BadRequest("Вклад обязан иметь дату закрытия");
            if (request.Type == AccountType.Deposit && request.DateCl <= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest("Дата закрытия вклада раньше/равна его открытия");
            if (request.Type == AccountType.Credit && !request.Rate.HasValue)
                return BadRequest("Кредит обязан иметь процентную ставку");
            if (request.Type == AccountType.Credit && !request.DateCl.HasValue)
                return BadRequest("Кредит обязан иметь дату закрытия");
            if (request.Type == AccountType.Credit && request.DateCl <= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest("Дата закрытия кредита раньше/равна его открытия");
            if (request.Type == AccountType.Credit && request.Balance <= 0)
                return BadRequest("Сумма выдаваемого кредита должна быть больше 0");
            var Account = new Account()
            {
                Id = Guid.NewGuid(),
                OwnerID = request.OwnerID,
                Type = request.Type,
                CType = request.CType,
                Balance = request.Balance,
                Rate = request.Rate,
                DateCr = DateOnly.FromDateTime(DateTime.UtcNow),
                DateCl = request.DateCl,
            };
            _repository.Create(Account);
            return CreatedAtAction(nameof(GetAccountById),
                     new { id = Account.Id },
                     Account);
        }

        [HttpPatch("{id}")]
        public IActionResult AccountPatch(Guid id, [FromBody] PatchAccount request)
        {
            var Account = _repository.GetById(id);
            if (Account == null) return NotFound();
            if (Account.Type == AccountType.Checking && request.Rate.HasValue)
                return BadRequest("Обычный счёт не может иметь процентную ставку");
            if (Account.Type == AccountType.Checking && request.DateCl.HasValue)
                return BadRequest("Обычный счет не может иметь дату закрытия");
            if (Account.Type == AccountType.Deposit && !request.Rate.HasValue)
                return BadRequest("Вклад обязан иметь процентную ставку");
            if (Account.Type == AccountType.Credit && !request.Rate.HasValue)
                return BadRequest("Кредит обязан иметь процентную ставку");
            if (Account.Type == AccountType.Deposit && !request.DateCl.HasValue)
                return BadRequest("Вклад обязан иметь дату закрытия");
            if (Account.Type == AccountType.Deposit && request.DateCl <= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest("Дата закрытия вклада раньше/равна его открытия");
            if (Account.Type == AccountType.Credit && !request.DateCl.HasValue)
                return BadRequest("Кредит обязан иметь дату закрытия");
            if (Account.Type == AccountType.Credit && request.DateCl <= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest("Дата закрытия кредита раньше/равна его открытия");
            if (request.Rate.HasValue)
                Account.Rate = request.Rate.Value;
            if (request.DateCl.HasValue)
                Account.DateCl = request.DateCl.Value;
            _repository.Update(Account);
            return Ok(Account);
        }

        [HttpPut("{id}")]
        public IActionResult AccountPut(Guid id, [FromBody] AccountCreateRequest request)
        {
            var Account = _repository.GetById(id);
            if (Account == null) return NotFound();
            if (Account.Type == AccountType.Checking && request.Rate.HasValue)
                return BadRequest("Обычный счёт не может иметь процентную ставку");
            if (Account.Type == AccountType.Checking && request.DateCl.HasValue)
                return BadRequest("Обычный счет не может иметь дату закрытия");
            if (Account.Type == AccountType.Deposit && !request.Rate.HasValue)
                return BadRequest("Вклад обязан иметь процентную ставку");
            if (Account.Type == AccountType.Deposit && !request.DateCl.HasValue)
                return BadRequest("Вклад обязан иметь дату закрытия");
            if (Account.Type == AccountType.Deposit && request.DateCl <= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest("Дата закрытия вклада раньше/равна его открытия");
            if (Account.Type == AccountType.Credit && !request.Rate.HasValue)
                return BadRequest("Кредит обязан иметь процентную ставку");
            if (Account.Type == AccountType.Credit && !request.DateCl.HasValue)
                return BadRequest("Кредит обязан иметь дату закрытия");
            if (Account.Type == AccountType.Credit && request.DateCl <= DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest("Дата закрытия кредита раньше/равна его открытия");
            if (Account.Type == AccountType.Credit && request.Balance <= 0)
                return BadRequest("Сумма выдаваемого кредита должна быть больше 0");
            Account.OwnerID = request.OwnerID;
            Account.Type = request.Type;
            Account.CType = request.CType;
            Account.Balance = request.Balance;
            Account.Rate = request.Rate;
            Account.DateCl = request.DateCl;
            _repository.Replace(Account);
            return Ok(Account);
        }

        [HttpDelete("{id}")]
        public IActionResult AccountDelete(Guid id)
        {
            var Account = _repository.GetById(id);
            if (Account == null) return NotFound();
            _repository.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}/transactions")]
        public IActionResult TransactionsGet(Guid id)
        {
            var Account = _repository.GetById(id);
            if (Account == null) return NotFound();
            var transactions = _repository.GetTransactions(id);
            return Ok(transactions);
        }

        [HttpPost("{id}/transactions")]
        public IActionResult TransactionsPost(Guid id, [FromBody] TransactionCreate request)
        {
            var Account = _repository.GetById(id);
            if (Account == null) return NotFound();
            switch (request.TType)
            {
                case TransactionType.Transfer:
                    if (!request.AccountID2.HasValue) return BadRequest("Для перевода укажите счёт-получатель");
                    if (request.AccountID2.Value == id) return BadRequest("Нельзя перевести деньги на тот же счёт");
                    if (request.Sum <= 0) return BadRequest("Сумма перевода должна быть больше нуля");
                    var Account2 = _repository.GetById(request.AccountID2.Value);
                    if (Account2 == null) return NotFound("Счёт-получатель не найден");
                    if (Account.Balance < request.Sum) return BadRequest("Недостаточно средств для перевода");
                    if (Account.CType != Account2.CType) return BadRequest("Типы валют счетов должны быть одинаковыми");
                    if (Account.CType != request.CType || Account2.CType != request.CType) return BadRequest("Валюта перевода отличается от валюты счета");
                    var transaction = new Transaction()
                    {
                        Id = Guid.NewGuid(),
                        AccountID = Account.Id,
                        AccountID2 = request.AccountID2,
                        Sum = request.Sum,
                        CType = request.CType,
                        TType = TransactionType.Transfer,
                        Description = request.Description,
                        DateCr = DateTime.UtcNow,
                    };
                    Account.Balance -= request.Sum;
                    Account2.Balance += request.Sum;
                    _repository.Update(Account);
                    _repository.AddTransaction(Account.Id, transaction);
                    _repository.AddTransaction(Account2.Id, transaction);
                    return CreatedAtAction(nameof(TransactionsGet),
                       new { accountId = Account.Id },
                       transaction);

                case TransactionType.Credit:
                case TransactionType.Debit:
                    if (request.AccountID2.HasValue) return BadRequest("Для зачисления/списания не указывайте второй счёт");
                    if (request.TType == TransactionType.Credit && Account.Balance < request.Sum) return BadRequest("Недостаточно средств для списания");
                    if (request.Sum <= 0) return BadRequest("Сумма для пополнения/списания должна быть больше нуля");
                    if (Account.CType != request.CType) return BadRequest("Валюта счета отличается от валюты зачисления/списания");
                    var transaction3 = new Transaction()
                    {
                        Id = Guid.NewGuid(),
                        AccountID = Account.Id,
                        AccountID2 = request.AccountID2,
                        Sum = request.Sum,
                        CType = request.CType,
                        TType = request.TType,
                        Description = request.Description,
                        DateCr = DateTime.UtcNow,
                    };
                    if (request.TType == TransactionType.Debit)
                    {
                        Account.Balance += request.Sum;
                        _repository.Update(Account);
                    }

                    else if (request.TType == TransactionType.Credit)
                    {
                        Account.Balance -= request.Sum;
                        _repository.Update(Account);
                    }
                    _repository.AddTransaction(Account.Id, transaction3);
                    return CreatedAtAction(nameof(TransactionsGet),
                       new { accountId = Account.Id },
                       transaction3);

                default:
                    return BadRequest("Неизвестный тип операции");
            }

        }
    }
}
