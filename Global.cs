namespace uwap.GameLibrary;

public static class Global
{
    public static ReaderWriterLockSlim ConsoleLock { get; set; } = new();
}