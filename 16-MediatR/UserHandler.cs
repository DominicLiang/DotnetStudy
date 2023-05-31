using MediatR;

namespace _16_MediatR;

public class UserHandler : NotificationHandler<NewUserNotification>
{
    protected override void Handle(NewUserNotification notification)
    {
        Console.WriteLine("新建了用户  " + notification.UserName + "  " + notification.Time);
    }
}

public class NameChangedHandler : NotificationHandler<UserNameChangeNotification>
{
    protected override void Handle(UserNameChangeNotification notification)
    {
        Console.WriteLine(notification.OldUserName + "  " + notification.NewUserName);
    }
}