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
using System.Globalization;

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

        public static List<Raider>[] ReadSheet()
        {

            var sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyAIpz0Rm9ywnetyh76B49uEesbo1jYQk6Y"
            });

            var spreadsheetId = "14fm2C7bpPJ7EzGTBdYFXHqL7KpSHGCpf8ktuNgLpn9o";
            // it's possible to add range to this variable 
            var sheetName = "Testsheet!A1:R31";

            // create the request to retrieve the data
            var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, sheetName);

            // execute the request and get the response
            var response = request.Execute();

            // extract the values from the response
            var values = response.Values;

            //var data = new List<List<string>>();

            // iterate over the values and do something with them

            List<Raider>[] raid = new List<Raider>[4];
            List<Raider> tokenGroup = new List<Raider>();
            int i = -1;
            int k = 0;
            bool findingItems = false;
            foreach (var row in values)
            {                
                //READ RAIDERS
                if (row[0] != "" && "DreadfulMysticVeneratedZenith-".Contains((string)row[0]))
                {
                    if (i >= 0)
                        raid[i] = tokenGroup;
                    i++;
                    tokenGroup = new List<Raider>();
                }
                else if (row[1] != "")
                {
                    tokenGroup.Add(new Raider((string)row[1], Class.Warrior, Role.Damage, new Status[] { StringToStatus((string)row[2]), StringToStatus((string)row[3]), StringToStatus((string)row[4]), StringToStatus((string)row[5]), StringToStatus((string)row[6]) }, new float[] { float.Parse(((string)row[11]).TrimEnd('%'), NumberStyles.Float, CultureInfo.InvariantCulture), float.Parse(((string)row[12]).TrimEnd('%'), NumberStyles.Float, CultureInfo.InvariantCulture) }));
                }


                if (findingItems && row[k] != "")
                {
                    Calculator.loot.Add(new Item(StringToSlot((string)row[k]), StringToStatus((string)row[k + 2])));
                }

                //READ LOOT
                if (!findingItems)
                {
                    foreach (var cell in row)
                    {
                        if (((string)row[k]).Equals("Slot") && ((string)row[k + 1]).Equals("Token") && ((string)row[k + 2]).Equals("ilvl"))
                        {
                            findingItems = true;
                            break;
                        }
                        k++;
                    }
                }



                //foreach (var cell in row)
                //{
                //    Console.Write($"{cell}\t");
                //}
                //Console.WriteLine("\n");
            }

            return raid;
        }

        private static Slot StringToSlot(string str)
        {
            switch (str)
            {
                case "Head": return Slot.Helm;
                case "Shoulders": return Slot.Shoulder;
                case "Chest": return Slot.Chest;
                case "Gloves": return Slot.Gloves;
                case "Legs": return Slot.Legs;
            }
            Console.WriteLine("Incorrect loot detected");
            return Slot.Helm;
        }

        private static Status StringToStatus(string str)
        {
            switch(str)
            {
                case "-": return Status.Empty;
                case "LFR": return Status.LFR;
                case "Normal": return Status.Normal;
                case "Heroic": return Status.Heroic;
                case "Mythic": return Status.Mythic;
            }

            return Status.Empty;
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
