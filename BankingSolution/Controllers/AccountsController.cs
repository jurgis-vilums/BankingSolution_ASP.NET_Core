using BankingSolution.Models;
using BankingSolution.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(Account account)
        {
            var createdAccount = await _accountService.CreateAccountAsync(account);
            return CreatedAtAction(nameof(GetAccountByNumber), new { accountNumber = createdAccount.AccountNumber }, createdAccount);
        }

        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<Account>> GetAccountByNumber(string accountNumber)
        {
            var account = await _accountService.GetAccountByNumberAsync(accountNumber);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            if (!accounts.Any())
            {
                return NotFound(accounts);
            }
            return Ok(accounts);
        }
    }
}
