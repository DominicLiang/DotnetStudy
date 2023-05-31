using MediatR;

namespace _16_MediatR;

public class WithReturnValue: IRequest<bool>
{
    public string msg { get; set; }
}
