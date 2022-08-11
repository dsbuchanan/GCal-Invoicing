
using Google.Apis.Calendar.v3.Data;
using System.Globalization;

namespace GCal_Invoicing
{
    internal class Shift
    {
        private const int ContactLine = 0;
        private const int RateLine = 1;
        private const int LunchLine = 2;

        private string storeCompany;
        private string storeName;
        private string storeLocation;
        private DateTime startTime;
        private DateTime endTime;
        private int weekOfYear;
        private string contact;
        private double hourlyRate;
        private int lunchMinutes;
        private double durationHours;

        public string StoreCompany
        {
            get { return storeCompany; }
        }

        public string StoreName
        {
            get { return storeName; }
        }
        
        public string StoreLocation
        {
            get { return storeLocation; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
        }

        public int WeekOfYear
        {
            get { return weekOfYear; }
        }

        public string Contact
        {
            get { return contact; }
        }

        public double HourlyRate
        {
            get { return hourlyRate; }
        }

        public double DurationHours
        {
            get { return durationHours; }
        }

        public void Print()
        {
            Console.WriteLine("Location: {0}, Start: {1}, End: {2}, Lunch mins: {3}, Dur hours: {4}, Hourly rate: {5}, WOY: {6}",
                storeName, startTime, endTime, lunchMinutes, durationHours, hourlyRate, weekOfYear);
        }

        private int GetThisWeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public Shift(Event ev)
        {
            storeCompany = ev.Summary.Substring(0, ev.Summary.IndexOf(':')).Trim();
            storeName = ev.Summary.Substring(ev.Summary.IndexOf(':') + 1).Trim();
            storeLocation = ev.Location;
            startTime = (DateTime) ev.Start.DateTime;
            endTime = (DateTime) ev.End.DateTime;
            weekOfYear = GetThisWeekOfYear(startTime);
            string[] lines = ev.Description.Replace("<br>", "\n").Split("\n");
            contact = lines[ContactLine].Substring(lines[ContactLine].IndexOf(':') + 1).Trim();
            hourlyRate = Convert.ToDouble(lines[RateLine].Substring(lines[RateLine].IndexOf('$') + 1, 
                lines[RateLine].IndexOf('/') - lines[RateLine].IndexOf('$') - 1));
            lunchMinutes = Convert.ToInt32(lines[LunchLine].Substring(lines[LunchLine].IndexOf(':') + 1,
                lines[LunchLine].IndexOf('m') - lines[LunchLine].IndexOf(':') - 1).Trim());
            durationHours = (endTime.Subtract(startTime).TotalMinutes - lunchMinutes) / 60;
        }
    }
}