namespace uwap.GameLibrary;

public class Level
{
    public int Width;

    public int Height;

    public int CursorOffsetY;

    public List<Thing>[,] Fields;

    public Func<ConsoleKey,bool> KeyFunction = key => true;

    public Level(int width, int height, List<Thing>[,] fields)
    {
        Width = width;
        Height = height;
        Fields = fields;
        CursorOffsetY = 0;
    }

    public void RedrawField(int x, int y)
    {
        Console.CursorLeft = x*2;
        Console.CursorTop += y - CursorOffsetY;
        CursorOffsetY = y;
        DrawField(x, y);
        Console.ResetColor();
    }

    public void ResetCursor()
    {
        Console.CursorLeft = 0;
        Console.CursorTop += Height - CursorOffsetY;
        CursorOffsetY = Height;
    }

    private void DrawField(int x, int y)
    {
        List<Thing> things = Fields[x, y];
        ConsoleColor? background = null;
        Content? content = null;
        foreach (Thing thing in things)
        {
            if (thing.BackgroundColor != null)
                background = thing.BackgroundColor;
            if (thing.Content != null)
                content = thing.Content;
        }
        if (background == null || content == null)
            Console.ResetColor();

        if (background != null)
            Console.BackgroundColor = background.Value;
            
        if (content != null)
        {
            Console.ForegroundColor = content.Color;
            Console.Write(content.Text);
        }
        else Console.Write("  ");
    }

    public void PrintToConsole()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                DrawField(x, y);
            }
            
            Console.ResetColor();
            Console.WriteLine();
        }
        CursorOffsetY = Height;
    }

    public void Run()
    {
        PrintToConsole();
        while (true)
        {
            if (KeyFunction(Console.ReadKey(true).Key))
            {
                return;
            }
        }
    }
}