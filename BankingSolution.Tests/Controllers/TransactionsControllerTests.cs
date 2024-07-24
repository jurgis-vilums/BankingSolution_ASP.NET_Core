using BankingSolution.Controllers;
using BankingSolution.Models;
using BankingSolution.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankingSolution.Tests.Controllers
{
    public class TransactionsControllerTests
    {
        private readonly Mock<IAccountService> _mockService;
        private readonly TransactionsController _controller;

        public TransactionsControllerTests()
        {
            _mockService = new Mock<IAccountService>();
            _controller = new TransactionsController(_mockService.Object);
        }

        [Fact]
        public async Task Deposit_AccountNotFound_ReturnsNotFound()
        {
            // Arrange
            var transaction = new Transaction { Account = "123456", Amount = 100 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transaction.Account))
                .ReturnsAsync((Account?)null);

            // Act
            var result = await _controller.Deposit(transaction);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("Account not found");
        }

        [Fact]
        public async Task Deposit_SuccessfulDeposit_ReturnsOk()
        {
            // Arrange
            var transaction = new Transaction { Account = "123456", Amount = 100 };
            var account = new Account { AccountNumber = "123456", Balance = 200 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transaction.Account))
                .ReturnsAsync(account);
            _mockService.Setup(service => service.UpdateAccountAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Deposit(transaction);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new Account { AccountNumber = "123456", Balance = 300 });
        }

        [Fact]
        public async Task Withdraw_AccountNotFound_ReturnsNotFound()
        {
            // Arrange
            var transaction = new Transaction { Account = "123456", Amount = 100 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transaction.Account))
                .ReturnsAsync((Account?)null);

            // Act
            var result = await _controller.Withdraw(transaction);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("Account not found");
        }

        [Fact]
        public async Task Withdraw_InsufficientFunds_ReturnsBadRequest()
        {
            // Arrange
            var transaction = new Transaction { Account = "123456", Amount = 100 };
            var account = new Account { AccountNumber = "123456", Balance = 50 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transaction.Account))
                .ReturnsAsync(account);

            // Act
            var result = await _controller.Withdraw(transaction);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Insufficient funds");
        }

        [Fact]
        public async Task Withdraw_SuccessfulWithdrawal_ReturnsOk()
        {
            // Arrange
            var transaction = new Transaction { Account = "123456", Amount = 100 };
            var account = new Account { AccountNumber = "123456", Balance = 200 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transaction.Account))
                .ReturnsAsync(account);
            _mockService.Setup(service => service.UpdateAccountAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Withdraw(transaction);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new Account { AccountNumber = "123456", Balance = 100 });
        }

        [Fact]
        public async Task Transfer_FromAccountNotFound_ReturnsNotFound()
        {
            // Arrange
            var transfer = new Transfer { FromAccount = "123456", ToAccount = "654321", Amount = 100 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transfer.FromAccount))
                .ReturnsAsync((Account?)null);

            // Act
            var result = await _controller.Transfer(transfer);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("Account not found");
        }

        [Fact]
        public async Task Transfer_ToAccountNotFound_ReturnsNotFound()
        {
            // Arrange
            var transfer = new Transfer { FromAccount = "123456", ToAccount = "654321", Amount = 100 };
            var account = new Account { AccountNumber = "123456", Balance = 200 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transfer.FromAccount))
                .ReturnsAsync(account);
            _mockService.Setup(service => service.GetAccountByNumberAsync(transfer.ToAccount))
                .ReturnsAsync((Account?)null);

            // Act
            var result = await _controller.Transfer(transfer);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().Be("Account not found");
        }

        [Fact]
        public async Task Transfer_InsufficientFunds_ReturnsBadRequest()
        {
            // Arrange
            var transfer = new Transfer { FromAccount = "123456", ToAccount = "654321", Amount = 300 };
            var fromAccount = new Account { AccountNumber = "123456", Balance = 200 };
            var toAccount = new Account { AccountNumber = "654321", Balance = 100 };
            _mockService.Setup(service => service.GetAccountByNumberAsync(transfer.FromAccount))
                .ReturnsAsync(fromAccount);
            _mockService.Setup(service => service.GetAccountByNumberAsync(transfer.ToAccount))
                .ReturnsAsync(toAccount);

            // Act
            var result = await _controller.Transfer(transfer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Insufficient funds");
        }

        [Fact]
        public async Task Transfer_SuccessfulTransfer_ReturnsOk()
        {
            // Arrange
            var transfer = new Transfer { FromAccount = "123456", ToAccount = "654321", Amount = 100 };
            var fromAccount = new Account { AccountNumber = "123456", Balance = 200 };
            var toAccount = new Account { AccountNumber = "654321", Balance = 100 };

            _mockService.Setup(service => service.GetAccountByNumberAsync(transfer.FromAccount))
                .ReturnsAsync(fromAccount);
            _mockService.Setup(service => service.GetAccountByNumberAsync(transfer.ToAccount))
                .ReturnsAsync(toAccount);
            _mockService.Setup(service => service.UpdateAccountAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Transfer(transfer);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new
                {
                    fromAccount = new Account { AccountNumber = "123456", Balance = 100 },
                    toAccount = new Account { AccountNumber = "654321", Balance = 200 }
                });
        }
    }
}
