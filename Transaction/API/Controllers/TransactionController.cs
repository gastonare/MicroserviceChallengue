using Microsoft.AspNetCore.Mvc;
using Transaction.Core.Interfaces;
using Transaction.Core.Models;

namespace Transaction.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateTransaction([FromBody] TransactionBody transactionBody)
        {
            var result = await _transactionService.CreateTransaction(transactionBody);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("")]
        public async Task<ActionResult> UpdateTransaction()
        {
            var result = _transactionService.UpdateTransactions();

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}