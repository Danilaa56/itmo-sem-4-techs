namespace Test_My_PLINQ;

public static class SingleQueue
{
    public static void Main()
    {
        IEnumerable<int> v = new List<int>();
        
        v.All(null);
        v.Any();
        v.Min();
        v.Max();
        v.Count();
        v.First();
        v.Last();
        v.Single();
        v.Select(a => a);
        v.Sum();
        v.Where(b => true);

        /*
         * All
         * Any
         * Min
         * Max
         * Count
         * First = Last = Single
         * Select
         * Sum
         * Where
         */

        /*
         * Aggregate
         * All
         * Any
         * Append
         * Average
         * Cast
         * Chunk
         * Concat
         * Count
         * Distinct
         * Except
         * First
         * Intersect
         * Join
         * Last
         * Max
         * Min
         * Prepand
         * Select       <-
         * Single
         * Skip
         * Sum
         * Take
         * Union
         * Where
         * Zip
         */
    }
}