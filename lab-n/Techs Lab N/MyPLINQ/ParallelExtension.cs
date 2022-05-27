namespace MyPLINQ;

public static class ParallelExtension
{
    public static MyParallelQuery<T> AsPara<T>(this IEnumerable<T> source)
    {
        return new MyParallelQuery<T>(source);
    }
    
    public static ParallelCollection<TSource> AsParallelCollection<TSource>(this IEnumerable<TSource> source)
    {
        return new ParallelCollection<TSource>(source);
    }
}