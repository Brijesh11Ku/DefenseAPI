using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DefenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly DAL _DataAccess;

        public LoginController(DAL DataAccess)
        {
            _DataAccess = DataAccess;
        }
    }
}
