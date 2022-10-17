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
            displayText.Text = "";

            if (Globals.calService == null) {
                Globals.RetryCred();
            }

            // Get events
            EventsResource.ListRequest request = Globals.calService.Events.List("primary");
            request.TimeMin = Globals.StartDate;
            request.TimeMax = Globals.EndDate;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            Events events = new Events();
            try
            {
                events = request.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            // Check for null
            if (events.Items == null || events.Items.Count == 0)
            {
                Console.WriteLine("No shifts found on these dates");
                invoiceCheckedListBox.Items.Clear();
                buttonCreateInvoices.Enabled = false;
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

            // Create invoices
            Globals.invoices.Clear();
            buttonCreateInvoices.Enabled = true;

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
                    Globals.invoices.Add(invoice);
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
                    Globals.invoices.Add(invoice);
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
                    Globals.invoices.Add(invoice);
                }
            }

            // CheckedListBox stuff            
            invoiceCheckedListBox.Items.Clear();

            var invoiceNameList = new List<string>();
            foreach (var invoice in Globals.invoices)
            {
                invoiceNameList.Add(
                    invoice.Number + " " +
                    invoice.Shifts[0].StoreCompany + ": " +
                    invoice.Shifts[0].StoreName + ": " +
                    invoice.Shifts.Count() + " shift(s)"
                );
                invoice.ConsolePrint();
                Console.WriteLine();
            }

            invoiceCheckedListBox.Items.AddRange(invoiceNameList.ToArray());
            for (var i = 0; i < invoiceCheckedListBox.Items.Count; i++)
            {
                invoiceCheckedListBox.SetItemChecked(i, true);
            }
        }

        private void buttonCreateInvoices_Click(object sender, EventArgs e)
        {
            displayText.Text = "";
            buttonGetInvoices.Enabled = false;
            buttonCreateInvoices.Enabled = false;
            int count = 0;
            for (var i = 0; i < Globals.invoices.Count; i++)
            {
                if (invoiceCheckedListBox.GetItemChecked(i))
                {
                    count++;
                    Console.WriteLine("Creating invoice {0} of {1}", count, invoiceCheckedListBox.CheckedItems.Count);
                    Console.WriteLine("{0} {1}: {2}",
                        Globals.invoices[i].Number,
                        Globals.invoices[i].Shifts[0].StoreCompany,
                        Globals.invoices[i].Shifts[0].StoreName
                    );
                    Globals.invoices[i].Print();
                    Console.WriteLine();
                }
            }
            Console.WriteLine("Done.");
            buttonGetInvoices.Enabled = true;
            buttonCreateInvoices.Enabled = true;
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