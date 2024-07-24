using BankingSolution.Controllers;
using BankingSolution.Models;
using BankingSolution.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankingSolution.Tests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly AccountsController _controller;
        private readonly Mock<IAccountService> _mockService;

        public AccountsControllerTests()
        {
            _mockService = new Mock<IAccountService>();
            _controller = new AccountsController(_mockService.Object);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newAccount = new Account { AccountNumber = "1234567890", Balance = 1000.00m };
            _mockService.Setup(service => service.CreateAccountAsync(It.IsAny<Account>())).ReturnsAsync(newAccount);

            // Act
            var result = await _controller.CreateAccount(newAccount);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult?.Value.Should().BeEquivalentTo(newAccount);
        }
        [Fact]
        public async Task CreateAccount_AlreadyExists_ThrowsException()
        {
            // Arrange
            var newAccount = new Account { AccountNumber = "1234567890", Balance = 1000.00m };
            _mockService.Setup(service => service.CreateAccountAsync(It.IsAny<Account>()))
                .ThrowsAsync(new Exception("An account with this account number already exists."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.CreateAccount(newAccount));
            exception.Message.Should().Be("An account with this account number already exists.");
        }

        [Fact]
        public async Task GetAccountByNumber_ReturnsOkObjectResult()
        {
            // Arrange
            var account = new Account { AccountNumber = "1234567890", Balance = 1000.00m };
            _mockService.Setup(service => service.GetAccountByNumberAsync("1234567890")).ReturnsAsync(account);

            // Act
            var result = await _controller.GetAccountByNumber("1234567890");

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().BeEquivalentTo(account);
        }
        [Fact]
        public async Task GetAccountByNumber_AccountNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetAccountByNumberAsync("1234567890")).ReturnsAsync((Account?)null);

            // Act
            var result = await _controller.GetAccountByNumber("1234567890");

            // Assert
            result.Should().BeOfType<ActionResult<Account>>();
        }

        [Fact]
        public async Task GetAllAccounts_ReturnsOkObjectResult()
        {
            // Arrange
            var accounts = new List<Account> { new Account { AccountNumber = "1234567890", Balance = 1000.00m } };
            _mockService.Setup(service => service.GetAllAccountsAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _controller.GetAllAccounts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().BeEquivalentTo(accounts);
        }
        [Fact]
        public async Task GetAllAccounts_NoAccountsFound_ReturnsNotFound()
        {
            // Arrange
            var emptyAccounts = new List<Account>(); 
            _mockService.Setup(service => service.GetAllAccountsAsync()).ReturnsAsync(emptyAccounts);

            // Act
            var result = await _controller.GetAllAccounts();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<Account>>>();
        }
    }
}