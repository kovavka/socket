using System;

namespace Domain
{
    [Serializable]
    public class EventDto
    {
        public DateTime Execution { get; set; }
        public string EventName { get; set; }
        public string EventComment { get; set; }
        public string EventInfo { get; set; }
        public string House { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string CityType { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}
