using MediatR;

namespace Cpa.Fas.ProductMs.Application.Common.Interfaces;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}