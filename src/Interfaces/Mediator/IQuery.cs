using MediatR;

namespace PhotoSi.Interfaces.Mediator
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}