
public interface IMessageService
{
    Task SaveMessageAsync(Message message);
    Task<IEnumerable<Message>> GetMessagesAsync(string groupName);
    Task MarkMessageAsReadAsync(int messageId);
}