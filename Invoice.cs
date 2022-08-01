using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCal_Invoicing
{
    abstract class Invoice
    {
        // all: invoice number, invoice date, bill to, shifts
        // specific: location, create in sheets
        private static Dictionary<string, string> companyCodes = new Dictionary<string, string>()
        {
            {"Bailey Nelson", "BN" },
            {"Medispecs", "MS" },
            {"Oscar Wylee", "OW" },
            {"Specsavers", "SS"}

        };
        protected string number;
        private DateTime date;
        private string contact;
        private List<Shift> shifts = new List<Shift>();

        public string Number
        {
            get { return number; }
        }
        
        public DateTime Date
        {
            get { return date; }
        }

        public string Contact
        {
            get { return contact; }
        }

        public List<Shift> Shifts
        {
            get { return shifts; }
        }

        public void AddShift(Shift shift)
        {
            this.shifts.Add(shift);
            this.shifts.Sort((a, b) => a.EndTime.CompareTo(b.EndTime));
            this.number = companyCodes[shifts[0].StoreCompany] + shifts[shifts.Count - 1].EndTime.Date.ToString("yyyyMMdd");
        }

        public abstract void Print();

        public Invoice(Shift shift)
        {
            this.date = DateTime.Now.Date;
            this.number = companyCodes[shift.StoreCompany] + shift.StartTime.Date.ToString("yyyyMMdd");
            this.contact = shift.Contact;
            this.shifts.Add(shift);
        }
    }
}
