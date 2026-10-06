namespace PAW.Web.Models;

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string ControllerName { get; set; } = string.Empty;
}
