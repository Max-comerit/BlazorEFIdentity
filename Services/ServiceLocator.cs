using BlazorEFIdentity.Components.Account.Pages;
using BlazorEFIdentity.Components.Account;
using BlazorEFIdentity.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFIdentity.Services
{
    public class ServiceLocator
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public ILogger<Login> Logger { get; }
        public NavigationManager NavigationManager { get; }
        public AuthenticationStateProvider AuthenticationStateProvider { get; }
        public ApplicationDbContext Context { get; }

        public ServiceLocator(
            UserManager<ApplicationUser> userManager,
            ILogger<Login> logger,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider,
            ApplicationDbContext context)
        {
            UserManager = userManager;
            Logger = logger;
            NavigationManager = navigationManager;
            AuthenticationStateProvider = authenticationStateProvider;
            Context = context;
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await Task.Run(() => UserManager.Users.ToList());
        }
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await UserManager.GetRolesAsync(user);
        }

        public async Task ToggleUserRoleAsync(ApplicationUser user, string role)
        {
            if (await UserManager.IsInRoleAsync(user, role))
            {
                await UserManager.RemoveFromRoleAsync(user, role);
            }
            else
            {
                await UserManager.AddToRoleAsync(user, role);
            }
        }

        public async Task AddAccountAsync(ApplicationUser user, Account account)
        {
            account.User = user;
            account.UserId = user.Id;

            Context.Accounts.Add(account);
            await Context.SaveChangesAsync();
        }
        public async Task<List<Account>> GetUserAccountsAsync(string userId)
        {
            return await Context.Accounts.Where(a => a.UserId == userId).ToListAsync();
        }
        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await Context.Accounts.FindAsync(accountId);
        }

        public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await Context.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
        }

        public async Task<Account> GetAccountByNumberAsync(int accountNumber)
        {
            return await Context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            Context.Transactions.Add(transaction);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            Context.Accounts.Update(account);
            await Context.SaveChangesAsync();
        }
        public async Task<ApplicationUser> GetUserByTransactionAsync(int transactionId)
        {
            var transaction = await Context.Transactions
                .Include(t => t.Account)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(t => t.Id == transactionId);

            return transaction?.Account?.User;
        }

        public async Task<ApplicationUser> GetUserByAccountAsync(int accountId)
        {
            var account = await Context.Accounts
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            return account?.User;
        }
    }
}
