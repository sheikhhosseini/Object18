namespace Core.Shared.Paging;

public class PageGenerator
{
    public static BasePaging Generate(int pageCount, int pageNumber, int take)
    {
        if (pageNumber <= 1) pageNumber = 1;
            
        return new BasePaging
        {
            ActivePage = pageNumber,
            PageCount = pageCount,
            PageId = pageNumber,
            TakeEntity = take,
            SkipEntity = (pageNumber - 1) * take,
            StartPage = pageNumber - 2 <= 0 ? 1 : pageNumber - 2,
            EndPage = pageNumber + 2 > pageCount ? pageCount : pageNumber + 2
        };
    }
}