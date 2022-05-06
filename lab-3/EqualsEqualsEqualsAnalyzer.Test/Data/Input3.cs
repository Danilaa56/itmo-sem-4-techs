using System;

public class Program
{
    public static void Main()
    {
        Ball b1 = new Ball(), b2 = new Ball();
        if (b1 == b2)
        {
            int x = 0;
        }
    }

    public class OpEquals
    {
        public static bool operator ==(OpEquals o1, OpEquals o2)
        {
            return true;
        }
    }
    
    public class Ball : OpEquals
    {
        public int x;
        
        public static void hello()
        {
        }
        
        public static bool operator +(Ball b1, Ball b2)
        {
            return new Ball();
        }
        
        public static bool operator !=(Ball b1, Ball b2)
        {
            return !(b1 == b2);
        }
    }
}