
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

        public void PrintShift()
        {
            //Console.WriteLine("Store Company: {0}, Name: {1}, Location: {2}", this.storeCompany,
            //    this.storeName, this.storeLocation);
            Console.WriteLine("Location: {0}, Start: {1}, End: {2}, Lunch mins: {3}, Dur hours: {4}, Hourly rate: {5}, WOY: {6}",
                this.storeName, this.startTime, this.endTime, this.lunchMinutes, this.durationHours, this.hourlyRate, this.weekOfYear);
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
            this.storeCompany = ev.Summary.Substring(0, ev.Summary.IndexOf(':')).Trim();
            this.storeName = ev.Summary.Substring(ev.Summary.IndexOf(':') + 1).Trim();
            this.storeLocation = ev.Location;
            this.startTime = (DateTime) ev.Start.DateTime;
            this.endTime = (DateTime) ev.End.DateTime;
            this.weekOfYear = GetThisWeekOfYear(this.startTime);
            string[] lines = ev.Description.Replace("<br>", "\n").Split("\n");
            this.contact = lines[ContactLine].Substring(lines[ContactLine].IndexOf(':') + 1).Trim();
            this.hourlyRate = Convert.ToDouble(lines[RateLine].Substring(lines[RateLine].IndexOf('$') + 1, 
                lines[RateLine].IndexOf('/') - lines[RateLine].IndexOf('$') - 1));
            this.lunchMinutes = Convert.ToInt32(lines[LunchLine].Substring(lines[LunchLine].IndexOf(':') + 1,
                lines[LunchLine].IndexOf('m') - lines[LunchLine].IndexOf(':') - 1).Trim());
            this.durationHours = (this.endTime.Subtract(this.startTime).TotalMinutes - this.lunchMinutes) / 60;
        }
    }
}