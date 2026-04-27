using System.Collections.Concurrent;
using Incoders.Template.Domain.SystemSettings;

namespace Incoders.Template.Infrastructure.Persistence.InMemory;

public sealed class InMemorySystemSettingStore
{
    private readonly ConcurrentDictionary<SystemSettingId, SystemSetting> _settings = new();

    internal ConcurrentDictionary<SystemSettingId, SystemSetting> Settings => _settings;
}
