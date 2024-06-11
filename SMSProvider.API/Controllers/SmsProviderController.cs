using Microsoft.AspNetCore.Mvc;
using SMSProvider.Service.Provider;
using SMSProvider.Service.Provider.Models;
using SMSProvider.Shared;

namespace SMSProvider.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SmsProviderController : ControllerBase
{
    private readonly ISmsProviderService _smsProviderService;

    public SmsProviderController(ISmsProviderService smsProviderService)
    {
        _smsProviderService = smsProviderService;
    }

    [HttpPost("add-provider")]
    public async Task<ActionResult<GetProviderResponseModel>> AddProvider([FromBody] ProviderRequestModel requestModel)
    {
        var response = await _smsProviderService.AddProvider(requestModel);
        return response.IsSuccess ? Ok(response) : BadRequest(response.Message);
    }

    [HttpPut("update-provider/{id}")]
    public async Task<ActionResult<GetProviderResponseModel>> UpdateProvider(int id, [FromBody] ProviderRequestModel requestModel)
    {
        var response = await _smsProviderService.UpdateProvider(id, requestModel);
        return response.IsSuccess ? Ok(response) : BadRequest(response.Message);
    }

    [HttpGet("get-providers")]
    public async Task<ActionResult<PaginatedOutPut<GetProviderResponseModel>>> GetProviders([FromQuery] BasePage basePage)
    {
        var response = await _smsProviderService.GetProviders(basePage);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}