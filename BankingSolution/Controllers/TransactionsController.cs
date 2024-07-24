using BankingSolution.Models;
using BankingSolution.Repositories;
using BankingSolution.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public TransactionsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] Transaction transaction)
        {
            var account = await _accountService.GetAccountByNumberAsync(transaction.Account);
            if (account == null)
            {
                return NotFound("Account not found");
            }

            account.Balance += transaction.Amount;
            await _accountService.UpdateAccountAsync(account);

            return Ok(account);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] Transaction transaction)
        {
            var account = await _accountService.GetAccountByNumberAsync(transaction.Account);
            if (account == null)
            {
                return NotFound("Account not found");
            }

            if (account.Balance < transaction.Amount)
            {
                return BadRequest("Insufficient funds");
            }

            account.Balance -= transaction.Amount;
            await _accountService.UpdateAccountAsync(account);

            return Ok(account);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] Transfer transfer)
        {
            var fromAccount = await _accountService.GetAccountByNumberAsync(transfer.FromAccount);
            var toAccount = await _accountService.GetAccountByNumberAsync(transfer.ToAccount);

            if (fromAccount == null || toAccount == null)
            {
                return NotFound("Account not found");
            }

            if (fromAccount.Balance < transfer.Amount)
            {
                return BadRequest("Insufficient funds");
            }

            fromAccount.Balance -= transfer.Amount;
            toAccount.Balance += transfer.Amount;

            await _accountService.UpdateAccountAsync(fromAccount);
            await _accountService.UpdateAccountAsync(toAccount);

            return Ok(new { fromAccount, toAccount });
        }
    }
}
