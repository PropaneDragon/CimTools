using System;
using System.Xml.Serialization;

namespace RushHour.Events
{
    public class CityEventData
    {
        public bool m_eventCreated = false;

        public bool m_eventStarted = false;

        public bool m_eventEnded = false;

        public int m_registeredCitizens = 0;

        public string m_eventName = "";

        public string m_creationDate = "";
    }
}
