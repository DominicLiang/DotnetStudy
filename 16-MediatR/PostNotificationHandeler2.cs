using MediatR;

namespace _16_MediatR;
public class PostNotificationHandler2 : NotificationHandler<PostNotification>
{
    protected override void Handle(PostNotification notification)
    {
        Console.WriteLine("2 " + notification.Body);
    }
}