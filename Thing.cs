namespace uwap.GameLibrary;

public abstract class Thing
{
    public abstract ConsoleColor? BackgroundColor { get; }

    public abstract Content? Content { get; }
}