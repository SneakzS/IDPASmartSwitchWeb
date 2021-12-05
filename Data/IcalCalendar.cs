using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using Radzen.Blazor;

namespace SmartSwitchWeb.Data
{
    public static class IcalCalendar
    {
        public static DateTime now = System.DateTime.Now;
        private static Calendar calendar = new Calendar();
        public static HashSet<Occurrence> addDailyEvent()
        {
            var occurences = new HashSet<Occurrence>();
            var later = now.AddHours(1);

            //Repeat daily for 5 days
            var rrule = new RecurrencePattern(FrequencyType.Daily, 1);

            var e = new CalendarEvent
            {
                Start = new CalDateTime(now),
                End = new CalDateTime(later),
                RecurrenceRules = new List<RecurrencePattern> { rrule },
                ExceptionDates = GetExceptionDates(1)
            };
            e.AddProperty(new CalendarProperty("X-Duration-Min", 20));
            e.AddProperty(new CalendarProperty("X-Usage-W", 20));

            calendar.Events.Add(e);
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1000);
            occurences = calendar.GetOccurrences(startDate, endDate);
            calendar.Events.Remove(e);
            //var calendar2 = Calendar.Load(calendar.ToString());
            return occurences;
        }
        private static List<PeriodList> GetExceptionDates(int daysOff)
        {
            return new List<PeriodList> { new PeriodList { new Period(new CalDateTime(now.AddDays(daysOff).Date)), new Period(new CalDateTime(now.AddDays(daysOff + 1).Date)) } };

        }
        public static HashSet<Occurrence> GetOccurrences(DateTime start, DateTime end)
        {
            return calendar.GetOccurrences(start, end);
        }
        public static IList<WorkLoadEvent> GetWorkloads(DateTime start, DateTime end)
        {
            IList<WorkLoadEvent> workLoads = new List<WorkLoadEvent>
            {
            new WorkLoadEvent { Start = DateTime.Today.AddDays(-2), End = DateTime.Today.AddDays(-2), Text = "Birthday", Duration = 4, MW = 5000},
            };
            return workLoads;

        }
    }
    public class WorkLoadEvent : AppointmentData
    {
        public int MW;
        public int Duration;
    }


}
