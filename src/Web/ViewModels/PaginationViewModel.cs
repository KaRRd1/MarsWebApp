namespace Web.ViewModels;

public class PaginationViewModel
{
    public PaginationViewModel(int totalItems, int currentPage, int itemsPerPage)
    {
        TotalItems = totalItems;
        CurrentPage = currentPage;
        ItemsPerPage = itemsPerPage;
        TotalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
    }

    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; private set; }
    public int Skip => (CurrentPage - 1) * ItemsPerPage;
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}