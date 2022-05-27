namespace MyPLINQ;

public static class AsParallelCollectionExtension
{
    public static ParallelCollection<TSource> AsParallelCollection<TSource>(this IEnumerable<TSource> source)
    {
        return new ParallelCollection<TSource>(source);
    }
}