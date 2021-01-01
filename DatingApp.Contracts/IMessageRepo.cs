using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Util.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Contracts
{
    public interface IMessageRepo : IGenericRepo<Message>
    {
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageTread(int userId, int recipientId);
    }
}
