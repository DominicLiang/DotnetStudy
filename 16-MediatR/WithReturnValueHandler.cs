using MediatR;

namespace _16_MediatR
{
    public class WithReturnValueHandler : IRequestHandler<WithReturnValue, bool>
    {
        public async Task<bool> Handle(WithReturnValue request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.msg == "hello");
        }
    }
}
