using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSwitchWeb.Data
{
    public static class IcalCalendar
    {

        private static Calendar calendar = new Calendar();
        public static void addDailyEvent()
        {
            if (calendar.Events.Count == 0)
            {
                DateTime now = System.DateTime.Now;
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
            }
        }
        private static List<PeriodList> GetExceptionDates(int daysOff)
        {
            return new List<PeriodList> { new PeriodList
            { new Period(new CalDateTime(DateTime.Now.AddDays(daysOff).Date)),
                new Period(new CalDateTime(DateTime.Now.AddDays(daysOff + 1).Date)) } };

        }
        public static HashSet<Occurrence> GetOccurrences(DateTime start, DateTime end)
        {
            return calendar.GetOccurrences(start, end);
        }
        public static IList<WorkLoadEvent> GetWorkloads(DateTime start, DateTime end)
        {
            addDailyEvent();
            var occurrences = GetOccurrences(start, end);

            IList<WorkLoadEvent> workLoads = new List<WorkLoadEvent>();
            foreach (var item in occurrences)
            {
                CalendarEvent sourceEvent = item.Source as CalendarEvent;

                int durationMin = (int)sourceEvent.Properties
                    .Where(i => i.Name == "X-Duration-Min")
                    .Select(x => x.Value).Single();
                int usageW = (int)sourceEvent.Properties
                    .Where(i => i.Name == "X-Usage-W").Select(x => x.Value).Single();
                workLoads.Add(new WorkLoadEvent
                {
                    Start = DateTime.Today.AddDays(1),
                    End = DateTime.Today.AddDays(12),
                    Text = "Vacation",
                    Duration = durationMin,
                    MW = usageW
                });
            }
            return workLoads;

        }
    }
    public class WorkLoadEvent
    {
        public int MW;
        public int Duration;
        public RepeatPattern Pattern;
        public int Guid;

        public DateTime Start { get; set; }
        public string Text { get; set; }
        public DateTime End { get; set; }
    }

}
