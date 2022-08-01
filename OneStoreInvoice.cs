using Google.Apis.Auth.OAuth2;
// using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;

namespace GCal_Invoicing
{
    internal class OneStoreInvoice : Invoice
    {
        public override void Print()
        {
            Console.WriteLine("Invoice number {0} for {1}: {2}", this.Number, this.Shifts[0].StoreCompany, this.Shifts[0].StoreName);
            Console.WriteLine("Invoice to: {0}, invoice date: {1}", this.Contact, this.Date.Date);
            foreach (var shift in this.Shifts)
            {
                shift.PrintShift();
            }

            // create new invoice spreadsheet from template
            var newSheet = new Google.Apis.Drive.v3.Data.File();
            newSheet.Name = this.Number;
            newSheet.Parents = new List<string> { "15jhiVeVDoiLzYWJgFQ3eea3J1ZVpJ-5q" };
            string sheetsTemplateId = "1CRRWwcM3yj9c6f2o_KEdmwOux8jeKRqrzpWZ2SDGqtE";
            Globals.driveService.Files.Copy(newSheet, sheetsTemplateId).Execute();

            // update a value
            String cell = "Invoice!B8";
            var valueRange = new ValueRange();
            var oblist = new List<object>() { this.Contact };
            valueRange.Values = new List<IList<object>> { oblist };
            var request = Globals.sheetsService.Spreadsheets.Values.Update(valueRange, newSheet.Id, cell);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            UpdateBandingRequest Val
            
        }

        public OneStoreInvoice(Shift shift) : base(shift)
        {

        }

    }
}
