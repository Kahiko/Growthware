using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GrowthWare.Test.Web.Support;

public class MockSession : ISession
{
    private readonly Dictionary<string, string> _storage = new Dictionary<string, string>();

    public string Id => "test-session-id";
    public bool IsAvailable => true;
    public IEnumerable<string> Keys => _storage.Keys;

    public void Clear() => _storage.Clear();

    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public void Remove(string key) => _storage.Remove(key);

    public void Set(string key, byte[] value) => _storage[key] = Convert.ToBase64String(value);

    public void SetString(string key, string value) => _storage[key] = value;

    public string? GetString(string key) => _storage.TryGetValue(key, out var value) ? value : null;

    public void Load() { }

    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
    {
        if (_storage.TryGetValue(key, out var stringValue))
        {
            value = Convert.FromBase64String(stringValue);
            return true;
        }
        value = null;
        return false;
    }

    public void Set(string key, byte[] value, TimeSpan? expiresIn) => Set(key, value);
    public void SetString(string key, string value, TimeSpan? expiresIn) => SetString(key, value);
}