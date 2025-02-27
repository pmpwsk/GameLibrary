namespace uwap.GameLibrary;

public class Level(int width, int height, int viewWidth, int viewHeight, List<Thing>[,] fields)
{
    public int Width = width;

    public int Height = height;

    public int ViewWidth = viewWidth;

    public int ViewHeight = viewHeight;

    public int ViewX = 0;
    
    public int ViewY = 0;

    public int CursorOffsetY = 0;

    public List<Thing>[,] Fields = fields;

    public Func<ConsoleKey,bool> KeyFunction = _ => true;

    public void RedrawField(int x, int y)
    {
        Global.ConsoleLock.EnterWriteLock();
        RedrawFieldWithoutLocking(x, y);
        Global.ConsoleLock.ExitWriteLock();
    }
    
    internal void RedrawFieldWithoutLocking(int x, int y)
    {
        if (x < ViewX || y < ViewY || x >= ViewX + ViewWidth || y >= ViewY + ViewHeight)
            return;
        
        Console.SetCursorPosition(
            (x - ViewX) * 2,
            CursorOffsetY + y - ViewY
        );
        DrawField(x, y);
        Console.ResetColor();
    }

    public void ResetCursor()
    {
        Console.SetCursorPosition(0, CursorOffsetY + ViewHeight);
    }

    private void DrawField(int x, int y)
    {
        List<Thing> things = Fields[x, y];
        ConsoleColor? background = null;
        Content? content = null;
        foreach (var thing in things)
        {
            if (thing.BackgroundColor != null)
            {
                background = thing.BackgroundColor;
                content = thing.Content;
            }
            else if (thing.Content != null)
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
        Console.Clear();
        
        for (int i = 0; i < CursorOffsetY; i++)
            Console.WriteLine();
        
        for (int y = ViewY; y < ViewY + ViewHeight; y++)
        {
            for (int x = ViewX; x < ViewX + ViewWidth; x++)
            {
                DrawField(x, y);
            }
            
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public void Run(Action? actionOnPrinted = null)
    {
        PrintToConsole();
        
        actionOnPrinted?.Invoke();
        
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
        Global.ConsoleLock.EnterWriteLock();
        
        if (x < 0)
            x = 0;
        if (x > Width - ViewWidth)
            x = Width - ViewWidth;
        if (y < 0)
            y = 0;
        if (y > Height - ViewHeight)
            y = Height - ViewHeight;
        
        ViewX = x;
        ViewY = y;
        PrintToConsole();
        
        Global.ConsoleLock.ExitWriteLock();
    }

    public void ScrollBy(int xOffset, int yOffset)
    {
        ScrollTo(ViewX + xOffset, ViewY + yOffset);
    }
}