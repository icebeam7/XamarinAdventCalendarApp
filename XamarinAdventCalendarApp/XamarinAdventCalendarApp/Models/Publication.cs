using XamarinAdventCalendarApp.Helpers;

namespace XamarinAdventCalendarApp.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string ScheduledDate { get; set; }
        public string Author { get; set; }
        public string AuthorURL { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }

        public ColorValues Colors { get; set; }
        public string AuthorImage => $"A{Id}.png";
    }
}
