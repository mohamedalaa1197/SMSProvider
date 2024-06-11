using Microsoft.AspNetCore.Mvc;
using SMSProvider.Service.Sms;
using SMSProvider.Service.SmsProvider;
using SMSProvider.Service.SmsProvider.Models;
using SMSProvider.Shared;

namespace SMSProvider.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SmsController : ControllerBase
{
    private readonly ISmsService _smsService;

    public SmsController(ISmsService smsService)
    {
        _smsService = smsService;
    }

    [HttpGet("logs")]
    public async Task<ActionResult<PaginatedOutPut<GetSmsLogResponseModel>>> GetSmsLogs([FromQuery] BasePage basePage)
    {
        var response = await _smsService.GetSmsLogs(basePage);
        return response.IsSuccess ? Ok(response) : BadRequest();
    }

    [HttpPost("send-sms")]
    public async Task<ActionResult<bool>> SendSms([FromBody] SendSmsRequestModel sendSmsRequestModel)
    {
        var response = await _smsService.SendSmsAsync(sendSmsRequestModel);
        return response.IsSuccess ? Ok(response) : BadRequest(response.Message);
    }
}