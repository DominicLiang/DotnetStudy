using MediatR;

namespace _16_MediatR
{
    public class NewUserNotification : INotification
    {
        public string UserName { get; set; }
        public DateTime Time { get; set; }
    }
}
