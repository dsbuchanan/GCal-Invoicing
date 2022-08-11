namespace GCal_Invoicing
{
    internal class SpecsaversInvoice : Invoice
    {
        private const string TemplateId = "1-1QcxHn0JIqx4K3ghbgy8zPxh7GATVLNFRTUrrB00b0";

        public override void Print()
        {
            // echo invoice data to console
            ConsolePrint();

            // create new Google sheet from template
            var gsheet = CreateCopyFromTemplate(TemplateId);

            // insert single cell field data
            UpdateNamedRange(Date, "InvoiceDate", gsheet.Id);
            UpdateNamedRange(Number, "InvoiceNo", gsheet.Id);
            UpdateNamedRange(Shifts[0].StoreCompany + ": " + Shifts[0].StoreName, "StoreName", gsheet.Id);

            // insert shifts data
            foreach (var shift in Shifts)
            {
                var shiftData = new List<IList<object>> { };
                var data = new List<object>();
                {
                    data.Add(shift.StartTime.ToString("dd/MM/yyyy"));
                    data.Add(shift.StartTime.DayOfWeek.ToString());
                    data.Add(shift.StartTime.ToString("HH:mm"));
                    data.Add(shift.EndTime.ToString("HH:mm"));
                    data.Add(shift.DurationHours);
                    data.Add("$" + shift.HourlyRate);
                }
                shiftData.Add(data);
                UpdateNamedRange(shiftData, shift.StartTime.DayOfWeek.ToString(), gsheet.Id);
            }

            // download Google sheet as pdf
            DownloadAsPDF(gsheet);
        }

        public SpecsaversInvoice(Shift shift) : base(shift)
        {

        }
    }
}
