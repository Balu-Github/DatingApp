using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IAuthService _userService;

        public ValuesController(IAuthService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]        
        public async Task<IActionResult> Get()
        {           
            var values = new string[] { "value1", "value2", "value3" };
            return Ok(values);
        }        
    }
}