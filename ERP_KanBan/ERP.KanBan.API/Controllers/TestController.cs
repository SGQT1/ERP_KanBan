using Microsoft.AspNetCore.Mvc;

namespace ERP.KanBan.API.Controllers;

public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    public IActionResult Get()
    {
        _logger.LogInformation("Test API called!");
        return Ok(new { Status = "Success", Message = "ERP API is running!" });
    }
}