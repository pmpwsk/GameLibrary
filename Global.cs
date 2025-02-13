using System.Diagnostics;

namespace uwap.GameLibrary;

public static class Global
{
    public static ReaderWriterLockSlim ConsoleLock { get; set; } = new();

    public static Stopwatch GameTime = new();
}