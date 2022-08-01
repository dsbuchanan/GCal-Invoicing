using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GCal_Invoicing
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Console.SetOut(new ControlWriter(displayText));
        }

        private void buttonGetShifts_Click(object sender, EventArgs e)
        {
            // Get events
            EventsResource.ListRequest request = Globals.calService.Events.List("primary");
            //Console.Write(Globals.StartDate.ToString() + ", ");
            //Console.WriteLine(Globals.EndDate.ToString());
            request.TimeMin = Globals.StartDate;
            request.TimeMax = Globals.EndDate;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            Events events = request.Execute();

            // Check for null
            // Console.WriteLine("Upcoming events:");
            if (events.Items == null || events.Items.Count == 0)
            {
                Console.WriteLine("No upcoming events found.");
                return;
            }
            else
            {
                // Filter events by valid practice                
                for (var i = 0; i < events.Items.Count; i++)
                {
                    if (!Globals.validPractices.Contains(events.Items[i].Summary))
                    {
                        events.Items.RemoveAt(i);
                        i--;
                    }
                }
                // List events
                //foreach (var ev in events.Items)
                //{
                //    Console.WriteLine("{0} ({1})", ev.Summary, ev.Start.DateTime.ToString());
                //}
            }
            
            var shifts = new List<Shift>();
            foreach (var ev in events.Items)
            {
                shifts.Add(new Shift(ev));
            }

            var invoices = new List<Invoice>();

            // Medispecs and Bailey Nelson?
            var result = shifts.Where(x => (x.StoreCompany == "Medispecs") || x.StoreCompany == "Bailey Nelson")
                                 .GroupBy(x => x.StoreCompany);
            if (result != null)
            {
                foreach (var company in result)
                {
                    OneStoreInvoice invoice = new OneStoreInvoice(company.ElementAt(0));
                    for (var i = 1; i < company.Count(); i++)
                    {
                        invoice.AddShift(company.ElementAt(i));
                    }
                    invoices.Add(invoice);
                }
            }

            // Specsavers
            var resultSS = shifts.Where(x => x.StoreCompany == "Specsavers")
                                 .GroupBy(x => new { x.StoreName, x.WeekOfYear });
            if (resultSS != null)
            {
                foreach (var storeweek in resultSS)
                {
                    OneStoreInvoice invoice = new OneStoreInvoice(storeweek.ElementAt(0));
                    for (var i = 1; i < storeweek.Count(); i++)
                    {
                        invoice.AddShift(storeweek.ElementAt(i));
                    }
                    invoices.Add(invoice);
                }
            }

            // Oscar Wylee
            var resultOW = shifts.Where(x => x.StoreCompany == "Oscar Wylee")
                                 .GroupBy(x => x.WeekOfYear);
            if (resultOW != null)
            {
                foreach (var week in resultOW)
                {
                    OneStoreInvoice invoice = new OneStoreInvoice(week.ElementAt(0));
                    for (var i = 1; i < week.Count(); i++)
                    {
                        invoice.AddShift(week.ElementAt(i));
                    }
                    invoices.Add(invoice);
                }
            }

            foreach (var invoice in invoices)
            {
                invoice.Print();
                Console.WriteLine();
            }
            
            /* OneStoreInvoice invoice = new OneStoreInvoice(new Shift(events.Items[0]));
            for (var i = 1; i < events.Items.Count; i++)
            {
                invoice.AddShift(new Shift(events.Items[i]));
            }
            invoice.Print(); */
        }
        
        private void displayText_TextChanged(object sender, EventArgs e)
        {
            displayText.SelectionStart = displayText.TextLength;
            displayText.ScrollToCaret();
        }
        private void monthCalendarDates_DateSelected(object sender, DateRangeEventArgs e)
        {
            Globals.StartDate = monthCalendarDates.SelectionRange.Start;
            Globals.EndDate = monthCalendarDates.SelectionRange.End;
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            displayText.Text = "";
        }
    }
}