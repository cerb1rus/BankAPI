using bankApi.Models;

namespace bankApi.Models
{
    public class AccountCreateRequest
    {
        public Guid OwnerID { get; set; }
        public AccountType Type { get; set; }
        public Сurrency CType { get; set; }
        public decimal Balance { get; set; }
        public decimal? Rate { get; set; }
        public DateOnly? DateCl { get; set; }
    }
}
