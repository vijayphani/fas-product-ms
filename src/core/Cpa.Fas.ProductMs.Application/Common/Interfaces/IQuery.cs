using MediatR;

namespace Cpa.Fas.ProductMs.Application.Common.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}