using MediatR;

namespace _16_MediatR;

public class PostNotification : INotification
{
    public string Body { get; set; }
}

