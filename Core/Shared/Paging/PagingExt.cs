namespace Core.Shared.Paging;

public static class PagingExt
{
    public static IQueryable<T> Paging<T> (this IQueryable<T> queryable, BasePaging pager)
    {
        return queryable.Skip(pager.SkipEntity).Take(pager.TakeEntity);
    }
}