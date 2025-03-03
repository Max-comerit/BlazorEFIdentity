namespace BlazorEFIdentity.Data
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public int AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public decimal Balance { get; set; }
        public int? CardNumber { get; set; }
        public int MyProperty { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<Transaction> Transactions { get; set; }

    }
}
