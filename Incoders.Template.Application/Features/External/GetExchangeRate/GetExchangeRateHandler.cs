using Incoders.Template.Application.Abstractions.ExternalServices;
using Incoders.Template.Domain.Common;
using MediatR;

namespace Incoders.Template.Application.Features.External.GetExchangeRate;

public sealed class GetExchangeRateHandler : IRequestHandler<GetExchangeRateQuery, Result<ExchangeRateResponse>>
{
    private readonly IExchangeRateGateway _gateway;

    public GetExchangeRateHandler(IExchangeRateGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Result<ExchangeRateResponse>> Handle(GetExchangeRateQuery request, CancellationToken cancellationToken)
    {
        var result = await _gateway.GetAsync(request.BaseCurrency, request.QuoteCurrency, cancellationToken);
        if (result.IsFailure)
        {
            return Result.Failure<ExchangeRateResponse>(result.Error);
        }

        return Result.Success(ExchangeRateResponse.FromDomain(result.Value));
    }
}
