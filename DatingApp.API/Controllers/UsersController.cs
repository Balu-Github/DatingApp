﻿using AutoMapper;
using DatingApp.API.Helpers;
using DatingApp.Contracts;
using DatingApp.DTO;
using DatingApp.Util.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService
            , IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]PageParams pageParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _userService.GetUser(currentUserId);
            pageParams.UserId = currentUserId;

            if(string.IsNullOrEmpty(pageParams.Gender))
            {
                pageParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _userService.GetUsers(pageParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users); 
            
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id < 0)
                return BadRequest();

            var updatedUser = await _userService.UpdateUser(id, userForUpdateDto);

            if (updatedUser == null)
                return Unauthorized();
            else
                return NoContent();           
        }
    }
}
