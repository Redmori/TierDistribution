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
using Newtonsoft.Json;

namespace TierDistribution
{
    public static class Input
    {

        //public static void ReadSheet2() {

        //    Axios.get("https://sheets.googleapis.com/v4/spreadsheets/14fm2C7bpPJ7EzGTBdYFXHqL7KpSHGCpf8ktuNgLpn9o/?key=AIzaSyAIpz0Rm9ywnetyh76B49uEesbo1jYQk6Y&includeGridData=true").then((res) =>
        //    {
        //        var data = res.data.sheets[0].data[0].rowData;
        //    });

        //}

        public static void ReadSheet()
        {

            var sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyAIpz0Rm9ywnetyh76B49uEesbo1jYQk6Y"
            });

            var spreadsheetId = "14fm2C7bpPJ7EzGTBdYFXHqL7KpSHGCpf8ktuNgLpn9o";
            // it's possible to add range to this variable like A1:F31
            var sheetName = "Tier&Embel!A1:F31";

            // create the request to retrieve the data
            var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, sheetName);

            // execute the request and get the response
            var response = request.Execute();

            // extract the values from the response
            var values = response.Values;

            var data = new List<List<string>>();

            // iterate over the values and do something with them
            var index = 0;
            foreach (var row in values)
            {
                Console.WriteLine($"---------------------------WE'RE ON ROW:{index}-------------------------------\n");
                index++;
                foreach (var cell in row)
                {
                    Console.Write($"{cell}\t");
                }
                Console.WriteLine("\n");
            }
        }
            //    var data = ReadData("AIzaSyBw_lIFMnJppjdhbKtNABGZQbjXS_lsDJw", "Testsheet!B1:C2");
            //    foreach (var row in data)
            //    {
            //        foreach (var cell in row)
            //        {
            //            Console.Write("{0}\t", cell);
            //        }
            //        Console.WriteLine();
            //    }
            //}

            //public static IList<IList<Object>> ReadData(string spreadsheetId, string range)
            //{
            //    UserCredential credential;

            //    using (var stream = new FileStream("client_secret_882081320166-teh8os6ppeli8drtrjseo448a4c9mf0v.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            //    {
            //        string credPath = "token.json";

            //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //            GoogleClientSecrets.Load(stream).Secrets,
            //            Scopes,
            //            "user",
            //            CancellationToken.None,
            //            new FileDataStore(credPath, true)).Result;
            //    }

            //    var service = new SheetsService(new BaseClientService.Initializer()
            //    {
            //        HttpClientInitializer = credential,
            //        ApplicationName = ApplicationName,
            //    });

            //    SpreadsheetsResource.ValuesResource.GetRequest request =
            //            service.Spreadsheets.Values.Get(spreadsheetId, range);

            //    ValueRange response = request.Execute();

            //    return response.Values;
            //}
        }
}
