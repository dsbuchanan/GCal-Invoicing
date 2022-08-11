namespace GCal_Invoicing
{
    internal class OscarWyleeInvoice : Invoice
    {
        private const string TemplateId = "1nbPJLK-99_7tG_-zasBYHaWttURyH6rk-LSmG4iad20";

        public override void Print()
        {
            // echo invoice data to console
            ConsolePrint();

            // create new Google sheet from template
            var gsheet = CreateCopyFromTemplate(TemplateId);

            // insert single cell field data
            UpdateNamedRange(Date, "InvoiceDate", gsheet.Id);
            UpdateNamedRange(Number, "InvoiceNo", gsheet.Id);

            // insert StoreList
            var storeList = new List<string>();
            foreach (var shift in Shifts)
            {
                storeList.Add(shift.StoreName);
            }
            storeList.Sort();
            UpdateNamedRange(String.Join("\n", storeList.Distinct().ToList()), "StoreList", gsheet.Id);

            // insert shifts data
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

        public OscarWyleeInvoice(Shift shift) : base(shift)
        {

        }
    }
}
