using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get(Guid senderGuid, Guid recieverGuid, float weight, int length, int width, int height)
        {
            Model.RequestModel requestModel = new Model.RequestModel(senderGuid, recieverGuid, weight, length, width, height);
            return await requestModel.CalculatePrice();
        }
    }
}
