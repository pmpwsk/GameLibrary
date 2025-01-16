namespace uwap.GameLibrary;

public class Level
{
    public int Width;

    public int Height;

    public int ViewWidth;

    public int ViewHeight;

    public int ViewX;
    
    public int ViewY;

    public int CursorOffsetY;

    public List<Thing>[,] Fields;

    public Func<ConsoleKey,bool> KeyFunction = key => true;

    public Level(int width, int height, int viewWidth, int viewHeight, List<Thing>[,] fields)
    {
        Width = width;
        Height = height;
        ViewWidth = viewWidth;
        ViewHeight = viewHeight;
        ViewX = 0;
        ViewY = 0;
        Fields = fields;
        CursorOffsetY = 0;
    }

    public void RedrawField(int x, int y)
    {
        if (x < ViewX || y < ViewY || x >= ViewX + ViewWidth || y >= ViewY + ViewHeight)
            return;
        
        Console.CursorLeft = (x - ViewX) * 2;
        Console.CursorTop += (y - ViewY) - CursorOffsetY;
        CursorOffsetY = y - ViewY;
        DrawField(x, y);
        Console.ResetColor();
    }

    public void ResetCursor()
    {
        Console.CursorLeft = 0;
        Console.CursorTop += ViewHeight - CursorOffsetY;
        CursorOffsetY = ViewHeight;
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
        for (int y = ViewY; y < ViewY + ViewHeight; y++)
        {
            for (int x = ViewX; x < ViewX + ViewWidth; x++)
            {
                DrawField(x, y);
            }
            
            Console.ResetColor();
            Console.WriteLine();
        }
        CursorOffsetY = ViewHeight;
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
    
    public void ScrollTo(int x, int y)
    {
        if (x >= 0 && x <= Width - ViewWidth && y >= 0 && y <= Height - ViewHeight)
        {   
            ViewX = x;
            ViewY = y;
            Console.CursorTop -= CursorOffsetY;
            PrintToConsole();
        }
    }

    public void ScrollBy(int xOffset, int yOffset)
    {
        ScrollTo(ViewX + xOffset, ViewY + yOffset);
    }
}