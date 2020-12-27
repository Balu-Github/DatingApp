using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
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

        [HttpGet]        
        public async Task<IActionResult> Get()
        {
            var values = await _userService.GetUsers();
            return Ok(values);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var values = await _userService.GetUsers();
            return Ok(values);
        }
    }
}