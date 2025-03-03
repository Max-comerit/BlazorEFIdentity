using System.Security.Cryptography.X509Certificates;

namespace BlazorEFIdentity.Data
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool IsReserved { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
    }
}
