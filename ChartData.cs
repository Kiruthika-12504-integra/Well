using System.Text.Json.Serialization;

namespace BlazorApp2.Components.Models
{
    public class ChartData
    {
        [JsonPropertyName("sleepHours")]
        public double SleepHours { get; set; }

        [JsonPropertyName("waterIntake")]
        public double WaterIntake { get; set; }

        [JsonPropertyName("exerciseMinutes")]
        public double ExerciseMinutes { get; set; }

        [JsonPropertyName("mealQuality")]
        public double MealQuality { get; set; }

        [JsonPropertyName("mood")]
        public double Mood { get; set; }

        [JsonPropertyName("energyLevel")]
        public double EnergyLevel { get; set; }
    }
}
