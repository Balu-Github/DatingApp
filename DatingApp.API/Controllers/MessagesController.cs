using AutoMapper;
using DatingApp.API.Helpers;
using DatingApp.Contracts;
using DatingApp.Data;
using DatingApp.DTO;
using DatingApp.Util.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepo _messageRepo;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageRepo messageRepo
            , IUserService userService
            , IMapper mapper)
        {
            _messageRepo = messageRepo;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _messageRepo.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();

            var _message = _mapper.Map<MessageToReturnDto>(messageFromRepo);

            return Ok(_message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageForuser(int userId, [FromQuery] MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = await _messageRepo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }


        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo = await _messageRepo.GetMessageTread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreationDto.SenderId = userId;

            var recipient = await _userService.GetUser(messageForCreationDto.RecipientId);

            if (recipient == null)
                return BadRequest("Could not find user");

            var sender = await _userService.GetUser(messageForCreationDto.SenderId); //TODO: revisit the logic

            var message = _mapper.Map<Message>(messageForCreationDto);

            var savedMessge = await _messageRepo.Add(message);

            var msg = await _messageRepo.GetMessage(savedMessge.Id);

            var messgeToReturn = _mapper.Map<MessageToReturnDto>(msg);

            return Ok(messgeToReturn);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _messageRepo.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                await _messageRepo.Delete(messageFromRepo);
            else
                await _messageRepo.Edit(messageFromRepo);

            return NoContent();
        }


        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _messageRepo.GetMessage(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _messageRepo.Edit(message);

            return NoContent();
        }
    }
}

