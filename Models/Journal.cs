using System;

namespace gsbMonolith.Models
{
    public class Journal
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string RelatedObject { get; set; }
        public string Description { get; set; }

        public Journal() { }

        public Journal(int? idUser, string eventName, DateTime date, string type, string relatedObject, string description)
        {
            IdUser = idUser;
            EventName = eventName;
            Date = date;
            Type = type;
            RelatedObject = relatedObject;
            Description = description;
        }
    }
}
