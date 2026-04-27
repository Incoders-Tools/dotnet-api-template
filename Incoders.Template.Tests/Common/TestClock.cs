using Incoders.Template.Application.Abstractions;

namespace Incoders.Template.Tests.Common;

internal sealed class TestClock : IClock
{
    public TestClock(DateTime utcNow)
    {
        UtcNow = utcNow;
    }

    public DateTime UtcNow { get; set; }
}
