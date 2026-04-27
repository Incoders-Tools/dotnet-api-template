using Incoders.Template.Domain.Common;
using MediatR;

namespace Incoders.Template.Application.Features.External.GetExchangeRate;

public sealed record GetExchangeRateQuery(string BaseCurrency, string QuoteCurrency)
    : IRequest<Result<ExchangeRateResponse>>;
