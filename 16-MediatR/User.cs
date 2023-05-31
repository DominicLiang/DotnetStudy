namespace _16_MediatR;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public DateTime CreateDateTime { get; set; }
    public int Credits { get; set; }

    private User() { }

    public User(string userName)
    {
        UserName = userName;
        CreateDateTime = DateTime.Now;
        Credits = 10;
        AddDomainEvent(new NewUserNotification() { UserName = userName, Time = DateTime.Now });
    }

    public void ChangeUserName(string newName)
    {
        string oldUser = UserName;
        UserName = newName;
        AddDomainEvent(new UserNameChangeNotification() { NewUserName = newName, OldUserName = oldUser });
    }
}
