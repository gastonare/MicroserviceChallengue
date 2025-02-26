using AntiFraud.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AntiFraud.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AntiFraudController : ControllerBase
    {
        private readonly IAntiFraudService _antiFraudService;

        public AntiFraudController(IAntiFraudService antiFraudService)
        {
            _antiFraudService = antiFraudService;
        }

        [HttpPost()]
        public async Task<ActionResult> CheckTransaction()
        {
            var result = await _antiFraudService.CheckTransactions();

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
