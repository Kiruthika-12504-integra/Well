using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorApp2.Components.Services;

public class WellnessService
{
    private readonly IJSRuntime _js;
    private readonly UserSessionService _session;

    public WellnessService(IJSRuntime js, UserSessionService session)
    {
        _js = js;
        _session = session;
    }

    private string GetUserKey()
    {
        if (string.IsNullOrWhiteSpace(_session.CurrentUsername))
            throw new InvalidOperationException("No user is currently logged in.");

        return $"wellnessHistory-{_session.CurrentUsername}";
    }

    public async Task<List<WellnessData>> GetWellnessHistoryAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", GetUserKey());
        return string.IsNullOrWhiteSpace(json)
            ? new List<WellnessData>()
            : JsonSerializer.Deserialize<List<WellnessData>>(json) ?? new List<WellnessData>();
    }

    public async Task SaveWellnessHistoryAsync(List<WellnessData> history)
    {
        var json = JsonSerializer.Serialize(history);
        await _js.InvokeVoidAsync("localStorage.setItem", GetUserKey(), json);
    }

    public async Task<List<WellnessData>> GetWeekRangeDataAsync(DateTime startOfWeek)
{
    var history = await GetWellnessHistoryAsync();
    var weekDates = Enumerable.Range(0, 7)
        .Select(offset => startOfWeek.AddDays(offset).ToString("yyyy-MM-dd"))
        .ToList();

    var weeklyData = weekDates.Select(date =>
    {
        var match = history.FirstOrDefault(h => h.Date == date);
        return match ?? new WellnessData { Date = date, IsMissing = true };
    }).ToList();

    return weeklyData;
}


    public WellnessAverages CalculateAverages(List<WellnessData> history)
    {
        var filled = history.Where(e => !e.IsMissing).ToList();
        if (!filled.Any()) return new WellnessAverages();

        return new WellnessAverages
        {
            SleepHours = Math.Round(filled.Average(e => e.SleepHours), 1),
            WaterIntake = Math.Round(filled.Average(e => e.WaterIntake), 1),
            ExerciseMinutes = (int)Math.Round(filled.Average(e => e.ExerciseMinutes)),
            MealQuality = Math.Round(filled.Average(e => e.MealQuality), 1),
            Mood = Math.Round(filled.Average(e => e.Mood), 1),
            EnergyLevel = Math.Round(filled.Average(e => e.EnergyLevel), 1)
        };
    }
}

public class WellnessData
{
    public string Date { get; set; } = "";
    public double SleepHours { get; set; }
    public double WaterIntake { get; set; }
    public int ExerciseMinutes { get; set; }
    public int MealQuality { get; set; }
    public int Mood { get; set; }
    public int EnergyLevel { get; set; }
    public string DailyNote { get; set; } = "";
    public bool IsMissing { get; set; } = false;
}

public class WellnessAverages
{
    public double SleepHours { get; set; }
    public double WaterIntake { get; set; }
    public int ExerciseMinutes { get; set; }
    public double MealQuality { get; set; }
    public double Mood { get; set; }
    public double EnergyLevel { get; set; }
}
