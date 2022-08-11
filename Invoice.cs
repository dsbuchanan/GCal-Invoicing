using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace GCal_Invoicing
{
    abstract class Invoice
    {
        // TODO: move these company codes to a company class
        private static Dictionary<string, string> companyCodes = new Dictionary<string, string>()
        {
            {"Bailey Nelson", "BN" },
            {"Medispecs", "MS" },
            {"Oscar Wylee", "OW" },
            {"Specsavers", "SS"}

        };
        private string number;
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

        private void SetNumber()
        {
            number = companyCodes[shifts[0].StoreCompany] + shifts[shifts.Count - 1].EndTime.Date.ToString("yyyyMMdd");
        }

        public void AddShift(Shift shift)
        {
            shifts.Add(shift);
            shifts.Sort((a, b) => a.EndTime.CompareTo(b.EndTime));
            SetNumber();
        }

        protected Google.Apis.Drive.v3.Data.File CreateCopyFromTemplate(string templateId)
        {
            var templateCopy = new Google.Apis.Drive.v3.Data.File();
            templateCopy.Name = number;
            templateCopy.Parents = new List<string> { "15jhiVeVDoiLzYWJgFQ3eea3J1ZVpJ-5q" };
            return Globals.driveService.Files.Copy(templateCopy, templateId).Execute();
        }

        protected void UpdateNamedRange(string value, string namedRange, string id)
        {
            var update = Globals.sheetsService.Spreadsheets.Values.Update(
                new ValueRange()
                {
                    Range = namedRange,
                    Values = new List<IList<object>>
                    {
                        new List<object>()
                        {
                            value
                        }
                    }
                },
                id, namedRange);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            update.Execute();
        }

        protected void UpdateNamedRange(DateTime value, string namedRange, string id)
        {
            UpdateNamedRange(value.ToString("dd/MM/yyyy"), namedRange, id);
        }

        protected void UpdateNamedRange(List<IList<object>> data, string namedRange, string id)
        {
            var update = Globals.sheetsService.Spreadsheets.Values.Update(
                new ValueRange()
                {
                    Range = namedRange,
                    Values = data
                },
                id, namedRange); ;
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            update.Execute();
        }

        protected void DownloadAsPDF(Google.Apis.Drive.v3.Data.File driveFile)
        {
            if (!Globals.downloadsEnabled)
            {
                return;
            }
            var request = Globals.driveService.Files.Export(driveFile.Id, "application/pdf");
            var stream = new MemoryStream();
            request.Download(stream);
            using (var file = new FileStream(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    + "\\Optometry\\Locations and Invoices\\" + driveFile.Name + ".pdf",
                FileMode.Create,
                FileAccess.Write))
            {
                stream.WriteTo(file);
            }

        }

        protected void ConsolePrint()
        {
            Console.WriteLine("Invoice number {0} for {1}: {2}", Number, Shifts[0].StoreCompany, Shifts[0].StoreName);
            Console.WriteLine("Invoice date: {0}", Date.ToString("dd/MM/yyyy"));
            foreach (var shift in Shifts)
            {
                shift.Print();
            }
        }

        public abstract void Print();

        public Invoice(Shift shift)
        {
            date = DateTime.Now.Date;
            contact = shift.Contact;
            AddShift(shift);
        }
    }
}
