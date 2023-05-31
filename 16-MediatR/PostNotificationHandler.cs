using MediatR;

namespace _16_MediatR;

public class PostNotificationHandler : NotificationHandler<PostNotification>
{
    protected override void Handle(PostNotification notification)
    {
        Console.WriteLine("1 "+notification.Body);
    }
}

