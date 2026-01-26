using BookingSystem.Application.DTOs;
using BookingSystem.Application.Interfaces;
using System.Text.Json;

namespace BookingSystem.Application.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly HttpClient _httpClient;

        public QuoteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.quotable.io/");
        }

        public async Task<QuoteDto?> GetRandomQuoteAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("random?tags=inspirational|success|sports");

                if (!response.IsSuccessStatusCode)
                    return GetFallbackQuote();

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<QuotableResponse>(json);

                if (apiResponse == null)
                    return GetFallbackQuote();

                // TRANSFORMATION: Add gaming context
                return new QuoteDto
                {
                    Content = apiResponse.content,
                    Author = apiResponse.author,
                    Category = "Motivation",
                    GamingContext = TransformToGamingContext(apiResponse.content),
                    Icon = "🎮"
                };
            }
            catch
            {
                return GetFallbackQuote();
            }
        }

        public async Task<QuoteDto?> GetActivityMotivationAsync(string activityName)
        {
            var quote = await GetRandomQuoteAsync();

            if (quote == null)
                return GetActivityFallback(activityName);

            // TRANSFORMATION: Customize based on activity
            quote.Icon = GetActivityIcon(activityName);
            quote.GamingContext = GetActivityContext(activityName);

            return quote;
        }

        // TRANSFORMATION LOGIC
        private string TransformToGamingContext(string originalQuote)
        {
            // Transform general quote to gaming context
            var keywords = new Dictionary<string, string>
            {
                { "success", "победа" },
                { "win", "victory" },
                { "practice", "тренирање" },
                { "challenge", "предизвик" },
                { "game", "игра" },
                { "play", "играње" }
            };

            foreach (var keyword in keywords)
            {
                if (originalQuote.ToLower().Contains(keyword.Key))
                {
                    return $"Во gaming светот: {keyword.Value} е клучот!";
                }
            }

            return "Секоја игра е нова авантура - уживај!";
        }

        private string GetActivityIcon(string activityName)
        {
            return activityName.ToLower() switch
            {
                var s when s.Contains("playstation") => "🎮",
                var s when s.Contains("пикадо") || s.Contains("darts") => "🎯",
                var s when s.Contains("билјард") || s.Contains("pool") => "🎱",
                var s when s.Contains("фудбалче") || s.Contains("fifa") => "⚽",
                _ => "🎲"
            };
        }

        private string GetActivityContext(string activityName)
        {
            return activityName.ToLower() switch
            {
                var s when s.Contains("playstation") => "Најдобрите играчи не се раѓаат - тие се креираат со практика!",
                var s when s.Contains("пикадо") || s.Contains("darts") => "Прецизноста бара фокус - секој фрли има своја цел!",
                var s when s.Contains("билјард") || s.Contains("pool") => "Стратегијата победува - размисли 3 потези напред!",
                var s when s.Contains("фудбалче") || s.Contains("fifa") => "Тимската работа прави го сонот реалност!",
                _ => "Играј со страст, победувај со стил!"
            };
        }

        private QuoteDto GetFallbackQuote()
        {
            var fallbackQuotes = new[]
            {
                new QuoteDto
                {
                    Content = "The only way to do great work is to love what you do.",
                    Author = "Steve Jobs",
                    Category = "Motivation",
                    GamingContext = "Играј со страст, победувај со љубов кон играта!",
                    Icon = "🎮"
                },
                new QuoteDto
                {
                    Content = "Success is not final, failure is not fatal: it is the courage to continue that counts.",
                    Author = "Winston Churchill",
                    Category = "Motivation",
                    GamingContext = "Секоја загубена игра е лекција за наредната победа!",
                    Icon = "🏆"
                },
                new QuoteDto
                {
                    Content = "It does not matter how slowly you go as long as you do not stop.",
                    Author = "Confucius",
                    Category = "Motivation",
                    GamingContext = "Практиката прави мајстори - никогаш не престани да се подобруваш!",
                    Icon = "⭐"
                }
            };

            var random = new Random();
            return fallbackQuotes[random.Next(fallbackQuotes.Length)];
        }

        private QuoteDto GetActivityFallback(string activityName)
        {
            return new QuoteDto
            {
                Content = "Winners never quit, and quitters never win.",
                Author = "Vince Lombardi",
                Category = "Sports",
                GamingContext = GetActivityContext(activityName),
                Icon = GetActivityIcon(activityName)
            };
        }

        // API Response Model
        private class QuotableResponse
        {
            public string content { get; set; } = "";
            public string author { get; set; } = "";
            public string[] tags { get; set; } = Array.Empty<string>();
        }
    }
}