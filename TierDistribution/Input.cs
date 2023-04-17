using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TierDistribution
{
    public static class Input
    {
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private static readonly string ApplicationName = "TierDistribution";


        public static void ReadSheet()
        {
            var data = ReadData("AIzaSyBw_lIFMnJppjdhbKtNABGZQbjXS_lsDJw", "Testsheet!B1:C2");
            foreach (var row in data)
            {
                foreach (var cell in row)
                {
                    Console.Write("{0}\t", cell);
                }
                Console.WriteLine();
            }
        }

        public static IList<IList<Object>> ReadData(string spreadsheetId, string range)
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret_882081320166-teh8os6ppeli8drtrjseo448a4c9mf0v.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();

            return response.Values;
        }
    }
}
