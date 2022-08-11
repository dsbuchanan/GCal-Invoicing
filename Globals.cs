using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GCal_Invoicing
{
    static class Globals
    {
        // debugging
        public static bool downloadsEnabled = true;
        
        // Google API
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly, SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive };
        static string ApplicationName = "GCal-Invoicing";
        static UserCredential credential;
        public static CalendarService calService;
        public static DriveService driveService;
        public static SheetsService sheetsService;
        public static List<string> validPractices;

        // calendar date picker
        private static DateTime startDate = DateTime.Now.Date;
        private static DateTime endDate = DateTime.Now.Date + new TimeSpan(23, 59, 59);

        public static DateTime StartDate
        {
            get { return Globals.startDate; }
            set { Globals.startDate = value; }
        }

        public static DateTime EndDate
        {
            get { return Globals.endDate; }
            set { Globals.endDate = value + new TimeSpan(23, 59, 59); }
        }

        static UserCredential GetCredential()
        {
            try
            {
                using (var stream = new FileStream(".\\credentials.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    // Console.WriteLine("Credential file saved to: " + credPath);
                    return credential;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        
        static List<string> GetPractices()
        {
            try
            {
                return File.ReadAllLines("practices.json").ToList();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static Globals()
        {
            credential = GetCredential();
            calService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
            driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
            sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
            validPractices = GetPractices();
        }
    }
}