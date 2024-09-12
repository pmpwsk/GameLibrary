namespace uwap.GameLibrary;

public class Level
{
    public int Width;

    public int Height;

    public List<Thing>[,] Fields;

    public Func<ConsoleKey,bool> KeyFunction = key => true;

    public Level(int width, int height, List<Thing>[,] fields)
    {
        Width = width;
        Height = height;
        Fields = fields;
    }

    public void PrintToConsole()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                List<Thing> things = Fields[x, y];
                if (things.Count == 0)
                {
                    Console.ResetColor();
                }
                else
                {
                    Console.BackgroundColor = things.Last().Color;
                }
                Console.Write("  ");
            }
            
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public void Run()
    {
        while (true)
        {
            PrintToConsole();
            if (KeyFunction(Console.ReadKey(true).Key))
            {
                return;
            }
            Console.CursorTop -= Height;
        }
    }
}