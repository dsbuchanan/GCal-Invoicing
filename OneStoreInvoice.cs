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
            var sheet = CreateCopyFromTemplate("1CRRWwcM3yj9c6f2o_KEdmwOux8jeKRqrzpWZ2SDGqtE");

            var recipient = new ValueRange() { Range = "Recipient" };
            var data = new List<object>() { this.Contact };
            recipient.Values = new List<IList<object>> { data };
            var update = Globals.sheetsService.Spreadsheets.Values.Update(recipient, sheet.Id, recipient.Range);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            update.Execute();
        }

        public OneStoreInvoice(Shift shift) : base(shift)
        {

        }

    }
}
