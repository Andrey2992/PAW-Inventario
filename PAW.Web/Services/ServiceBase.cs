namespace PAW.Web.Services;

public abstract class ServiceBase
{
    protected string BaseUrl { get; set; } = "https://localhost:7038/";

    protected string SetPathUrl(string name) => $"{BaseUrl}{name}";
}
