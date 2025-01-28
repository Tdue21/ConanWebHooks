using ConanWebHooks.Models;
using Newtonsoft.Json;

namespace ConanWebHooks.Services;

public class SettingsService(IDataService dataService)
{
    private readonly IDataService _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

    public async Task<Settings> GetSettings()
    {
        var data = await _dataService.ReadData("settings.json");
        if (data != null)
        {
            var result = JsonConvert.DeserializeObject<Settings>(data);
            return result ?? new Settings();
        }
        return new Settings();
    }
}
