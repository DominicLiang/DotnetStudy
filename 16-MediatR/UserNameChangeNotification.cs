using MediatR;

namespace _16_MediatR;

public class UserNameChangeNotification:INotification
{
    public string OldUserName { get; set; }
    public string NewUserName { get; set; }
}
