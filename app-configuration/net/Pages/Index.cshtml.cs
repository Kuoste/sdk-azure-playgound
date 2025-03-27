using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TestAppConfig;

namespace net.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public Settings Settings { get; }

    public IndexModel(IOptionsSnapshot<Settings> options, ILogger<IndexModel> logger)
    {
        Settings = options.Value;
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
