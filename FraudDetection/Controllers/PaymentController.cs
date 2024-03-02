using AdminAuth.Models.PaymentModel;
using AdminAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AdminAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private IConfiguration _config;

        //Inject the configuration into the Login controller
        public PaymentController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ProcessPayment([FromBody] PaymentData paymentData)
        {
            AccountRepository accountRepository = new AccountRepository();
            if (paymentData == null)
            {
                return BadRequest("Invalid payload");
            }

            // Extract parameters from the model
            string accountNumber = paymentData.AccountNumber;
            string transactionType = paymentData.TransactionType;
            decimal amount = paymentData.Amount;

            // Validate parameters
            if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(transactionType) || amount <= 0)
            {
                return BadRequest("Missing required parameters");
            }

            // Performs the business logic based on the transaction type
            switch (transactionType.ToLower())
            {
                case "transfer":
                    return ProcessTransfer(senderAccountNumber, recipientAccountNumber, transferAmount);
                case "credit":
                    return ProcessCredit(accountNumber, amount);
                case "withdrawal":
                    return ProcessWithdrawal(accountNumber, amount);
                default:
                    return BadRequest("Invalid transaction type");
            }

            /*accountRepository.UpdateAccountBalance();*/
        }

        public string senderAccountNumber = "123456789";
        public string recipientAccountNumber = "987654321";
        public decimal transferAmount = 100;

        private IActionResult ProcessTransfer(string senderAccountNumber, string recipientAccountNumber, decimal amount)
        {
            var accountRepository = new AccountRepository();

            // Call the ProcessTransfer method with the account numbers and transfer amount
            /*var result = ProcessTransfer(senderAccountNumber, recipientAccountNumber, transferAmount);*/


            var senderAccount = accountRepository.GetAccountByAccountNumber(senderAccountNumber);
            var recipientAccount = accountRepository.GetAccountByAccountNumber(recipientAccountNumber);

            if (senderAccount == null || recipientAccount == null)
            {
                return BadRequest("Sender or recipient account not found");
            }

            if (senderAccount.Balance < amount)
            {
                return BadRequest("Insufficient balance for transfer");
            }

            senderAccount.Balance -= amount;
            recipientAccount.Balance += amount;

            return Ok("Transfer successful");
        }


        private IActionResult ProcessCredit(string accountNumber, decimal amount)
        {
            var accountRepository = new AccountRepository();
            // Retrieves the account from the repository based on the account number
            var account = accountRepository.GetAccountByAccountNumber(accountNumber);

            // Check if the account exists
            if (account == null)
            {
                return BadRequest("Account not found");
            }

            // Perform the credit operation
            account.Balance += amount;

            // Save changes (if applicable)

            // Return a success response
            return Ok("Credit successful");
        }


        private IActionResult ProcessWithdrawal(string accountNumber, decimal amount)
        {
            var accountRepository = new AccountRepository();
            // Retrieve the account from the repository based on the account number
            var account = accountRepository.GetAccountByAccountNumber(accountNumber);

            // Check if the account exists
            if (account == null)
            {
                return BadRequest("Account not found");
            }

            // Check if the account has sufficient balance for withdrawal
            if (account.Balance < amount)
            {
                return BadRequest("Insufficient balance for withdrawal");
            }

            // Perform the withdrawal operation
            account.Balance -= amount;

            // Save changes (if applicable)

            // Return a success response
            return Ok("Withdrawal successful");
        }


        // Methods for handling specific transaction types
        // These methods can be similar to what was shown in the previous response
    }




}

