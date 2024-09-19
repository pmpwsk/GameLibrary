using uwap.GameLibrary;

public abstract class MovingThing : Thing
{

    private Level Level;

    private int X;
    
    private int Y;

    public MovingThing(Level level, int x, int y)
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

            List<Thing> oldField = Level.Fields[X, Y];
            oldField.Remove(this);
            Level.RedrawField(X, Y);

            List<Thing> newField = Level.Fields[x, y];
            newField.Add(this);
            Level.RedrawField(x, y);

            Level.ResetCursor();
            X = x;
            Y = y;

            bool stop = false;
            foreach (Thing thing in oldField)
                if (thing is LeaveEventThing t && t.OnLeave(this))
                    stop = true;
            foreach (Thing thing in newField)
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