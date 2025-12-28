namespace bankApi.Models
{
    public class TransactionCreate
    {
        public Guid AccountID { get; set; }
        public Guid? AccountID2 { get; set; }
        public decimal Sum { get; set; }
        public Сurrency CType { get; set; }
        public TransactionType TType { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
