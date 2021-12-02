using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;

namespace SmartSwitchWeb.Data
{
    public static class IcalCalendar
    {
        public static HashSet<Occurrence> occurences;
        public static DateTime now = System.DateTime.Now;
        public static HashSet<Occurrence> addDailyEvent()
        {
            
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

            var calendar = new Calendar();
            calendar.Events.Add(e);
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1000);
            occurences = calendar.GetOccurrences(startDate,endDate);
            return occurences;
        }
        private static List<PeriodList> GetExceptionDates(int daysOff)
        {
            return new List<PeriodList> { new PeriodList { new Period(new CalDateTime(now.AddDays(daysOff).Date)), new Period(new CalDateTime(now.AddDays(daysOff+1).Date)) } };

        }
    }
}
