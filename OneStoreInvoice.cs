namespace GCal_Invoicing
{
    internal class OneStoreInvoice : Invoice
    {
        private const string TemplateId = "1CRRWwcM3yj9c6f2o_KEdmwOux8jeKRqrzpWZ2SDGqtE";

        public override async void Print()
        {
            // echo invoice data to console
            Console.WriteLine("Invoice number {0} for {1}: {2}", Number, Shifts[0].StoreCompany, Shifts[0].StoreName);
            Console.WriteLine("Invoice to: {0}, invoice date: {1}", Contact, Date.Date);
            foreach (var shift in Shifts)
            {
                shift.Print();
            }

            // create new Google sheet from template
            var gsheet = CreateCopyFromTemplate(TemplateId);

            // insert single cell field data
            UpdateNamedRange(Date, "InvoiceDate", gsheet.Id);
            UpdateNamedRange(Number, "InvoiceNo", gsheet.Id);
            UpdateNamedRange(Contact, "RecipientName", gsheet.Id);
            UpdateNamedRange(Shifts[0].StoreLocation.Replace(", ", "\n"), "Recipient", gsheet.Id);
            // insert shifts data
            // TODO: implement limit on number of shifts per invoice
            var shiftsData = new List<IList<object>> { };
            foreach (var shift in Shifts)
            {
                var shiftData = new List<object>();
                {
                    shiftData.Add(shift.StartTime.ToString("dd/MM/yyyy"));
                    shiftData.Add(shift.StoreName);
                    shiftData.Add(shift.StartTime.ToString("HH:mm"));
                    shiftData.Add(shift.EndTime.ToString("HH:mm"));
                    shiftData.Add(shift.DurationHours);
                    shiftData.Add("$" + shift.HourlyRate + "/hour");
                    shiftData.Add(shift.DurationHours * shift.HourlyRate);
                }
                shiftsData.Add(shiftData);
            }
            UpdateNamedRange(shiftsData, "Shifts", gsheet.Id);

            // download Google sheet as pdf
            DownloadAsPDF(gsheet);
        }

        public OneStoreInvoice(Shift shift) : base(shift)
        {

        }

    }
}
