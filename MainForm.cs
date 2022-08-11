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
            request.TimeMin = Globals.StartDate;
            request.TimeMax = Globals.EndDate;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            Events events = request.Execute();

            // Check for null
            if (events.Items == null || events.Items.Count == 0)
            {
                Console.WriteLine("No shifts found on these dates");
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
            }
            
            var shifts = new List<Shift>();
            foreach (var ev in events.Items)
            {
                shifts.Add(new Shift(ev));
            }

            var invoices = new List<Invoice>();

            // Default (including Medispecs and Bailey Nelson)
            var result = shifts.Where(x => (x.StoreCompany == "Medispecs") || x.StoreCompany == "Bailey Nelson")
                                 .GroupBy(x => x.StoreCompany);
            if (result != null)
            {
                foreach (var company in result)
                {
                    var invoice = new DefaultInvoice(company.ElementAt(0));
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
                    var invoice = new SpecsaversInvoice(storeweek.ElementAt(0));
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
                    var invoice = new OscarWyleeInvoice(week.ElementAt(0));
                    for (var i = 1; i < week.Count(); i++)
                    {
                        invoice.AddShift(week.ElementAt(i));
                    }
                    invoices.Add(invoice);
                }
            }

            int invoiceCount = 1;
            foreach (var invoice in invoices)
            {
                Console.WriteLine("Creating invoice {0} of {1}", invoiceCount, invoices.Count);
                invoice.Print();
                Console.WriteLine();
                invoiceCount++;
            }

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

        private void downloadsEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (downloadsEnabledCheckBox.Checked)
            {
                Globals.downloadsEnabled = true;
            } else
            {
                Globals.downloadsEnabled = false;
            }
        }
    }
}