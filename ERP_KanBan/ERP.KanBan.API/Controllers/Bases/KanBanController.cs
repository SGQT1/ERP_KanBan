using Microsoft.AspNetCore.Mvc;

namespace ERP.KanBan.API.Controllers.Bases
{
    [Route("kanban/[controller]")]
    public abstract class KanBanController : BaseController
    {
        
    }
}