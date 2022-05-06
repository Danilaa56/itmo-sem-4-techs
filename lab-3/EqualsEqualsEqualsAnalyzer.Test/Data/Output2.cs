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
    
    public static class Ball
    {
        public int x;
        
        public static void hello()
        {
        }
        
        public static bool operator +(Ball b1, Ball b2)
        {
            return new Ball();
        }
        
        public static bool operator ==(Ball b1, Ball b2)
        {
            return b1.x == b2.x;
        }
        
        public static bool operator !=(Ball b1, Ball b2)
        {
            return !(b1 == b2);
        }
    }
}