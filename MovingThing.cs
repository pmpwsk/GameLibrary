namespace uwap.GameLibrary;

public abstract class MovingThing : Thing
{
    protected readonly Level Level;

    public int X;
    
    public int Y;

    public virtual ConsoleColor? BackgroundColor => throw new NotImplementedException();

    public virtual Content? Content => throw new NotImplementedException();

    protected MovingThing(Level level, int x, int y)
    {
        Level = level;
        X = x;
        Y = y;
    }

    public bool MoveTo(int x, int y)
    {
        if (x >= 0 && x < Level.Width && y >= 0 && y < Level.Height)
        {
            if (Level.Fields[x, y].Any(obj => obj is SolidThing))
                return false;
            
            Global.ConsoleLock.EnterWriteLock();

            List<Thing> oldField = Level.Fields[X, Y];
            oldField.Remove(this);
            Level.RedrawFieldWithoutLocking(X, Y);

            List<Thing> newField = Level.Fields[x, y];
            newField.Add(this);
            Level.RedrawFieldWithoutLocking(x, y);

            Level.ResetCursor();
            
            Global.ConsoleLock.ExitWriteLock();
            
            X = x;
            Y = y;

            bool stop = false;
            foreach (var thing in oldField)
                if (thing is LeaveEventThing t && t.OnLeave(this))
                    stop = true;
            foreach (var thing in newField)
                if (thing != this && thing is EnterEventThing t && t.OnEnter(this))
                    stop = true;
            return stop;
        }
        
        return false;
    }

    public bool MoveBy(int xOffset, int yOffset)
    {
        return MoveTo(X + xOffset, Y + yOffset);
    }
}