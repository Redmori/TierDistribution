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

        public static async Task<List<List<string>>> ReadData(string spreadsheetId, string apiKey, int sheetIndex)
        {
            using var client = new HttpClient();

            var response = await client.GetAsync($"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/?key={apiKey}&includeGridData=true");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to read data from Google Sheets API. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);

            var sheetsResponse = JsonConvert.DeserializeObject<GoogleSheetsApiResponse>(content);

            if (sheetIndex >= sheetsResponse.Sheets.Count)
            {
                throw new Exception($"Invalid sheet index: {sheetIndex}");
            }

            var rowData = sheetsResponse.Sheets[sheetIndex].Data.RowData;

            var data = new List<List<string>>();

            foreach (var row in rowData)
            {
                var rowDataList = new List<string>();

                foreach (var cell in row.Values)
                {
                    rowDataList.Add(cell.FormattedValue ?? "");
                }

                data.Add(rowDataList);
            }

            return data;
        }

        private class GoogleSheetsApiResponse
        {
            public List<GoogleSheetsApiSheet> Sheets { get; set; }
        }

        private class GoogleSheetsApiSheet
        {
            public GoogleSheetsApiSheetData Data { get; set; }
        }

        private class GoogleSheetsApiSheetData
        {
            public List<GoogleSheetsApiRow> RowData { get; set; }
        }

        private class GoogleSheetsApiRow
        {
            public List<GoogleSheetsApiCell> Values { get; set; }
        }

        private class GoogleSheetsApiCell
        {
            public string FormattedValue { get; set; }
        }



        //private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        //private static readonly string ApplicationName = "TierDistribution";

        //public static void ReadSheet()
        //{
        //    var data = ReadData("14fm2C7bpPJ7EzGTBdYFXHqL7KpSHGCpf8ktuNgLpn9o", "Testsheet!B1:C2");
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
