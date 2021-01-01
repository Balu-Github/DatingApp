using DatingApp.Contracts;
using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Repo.Generic;
using DatingApp.Util.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Repo
{
    public class MessageRepo : GenericRepo<Message>, IMessageRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        public MessageRepo(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _unitOfWork.Context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _unitOfWork.Context.Messages.AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;

                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;

                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId 
                                && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageTread(int userId, int recipientId)
        {
            var messages = await _unitOfWork.Context.Messages
                                 .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
                                        && m.SenderId == recipientId
                                    || m.RecipientId == recipientId && m.SenderId == userId
                                    && m.SenderDeleted == false)
                                 .OrderByDescending(m => m.MessageSent)
                                 .ToListAsync();
            return messages;
        }
    }
}
