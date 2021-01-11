using MediatR;

namespace PhotoSi.Interfaces.Mediator
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}