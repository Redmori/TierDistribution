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


        //public static void WriteSheet()
        //{
        //    var sheetsService = new SheetsService(new BaseClientService.Initializer
        //    {
        //        ApiKey = "AIzaSyAIpz0Rm9ywnetyh76B49uEesbo1jYQk6Y"
        //    });

        //    var spreadsheetId = "14fm2C7bpPJ7EzGTBdYFXHqL7KpSHGCpf8ktuNgLpn9o";
        //    var range = "Testsheet!A33:B34";
        //    var values = new ValueRange
        //    {
        //        Values = new List<IList<object>> {
        //        new List<object> { "Value1A", "Value1B" },
        //        new List<object> { "Value2A", "Value2B" },
        //        },
        //    };
        //    var updateRequest = sheetsService.Spreadsheets.Values.Update(values, spreadsheetId, range);
        //    updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        //    var updateResponse = updateRequest.Execute();
        //}
        public static (List<Raider>[],List<Item>[]) ReadSheet()
        {

            var sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyAIpz0Rm9ywnetyh76B49uEesbo1jYQk6Y"
            });

            var spreadsheetId = "14fm2C7bpPJ7EzGTBdYFXHqL7KpSHGCpf8ktuNgLpn9o";
            // it's possible to add range to this variable 
            var sheetName = "Testsheet!A1:AA31";

            // create the request to retrieve the data
            var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, sheetName);

            // execute the request and get the response
            var response = request.Execute();

            // extract the values from the response
            var values = response.Values;

            //var data = new List<List<string>>();

            // Data columns:
            int indexName = 1;
            int indexClass = 2;
            int indexRole = 3;
            int indexGearHelm = 4;
            int indexTier2p = 13;
            int indexTier4p = indexTier2p + 1;
            int indexOmni = 9;
            int indexVault = 10;

            int indexValueHelm = 16;


            // iterate over the values and do something with them

            List<Raider>[] raid = new List<Raider>[4];
            List<Raider> tokenGroup = new List<Raider>();
            int i = -1;

            List<Item>[] newLoot = new List<Item>[5];
            for (int j = 0; j < newLoot.Length; j++)
            {
                newLoot[j] = new List<Item>();
            }
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
                    float[] itemValues = new float[] { StringToValue((string)row[indexValueHelm]), StringToValue((string)row[indexValueHelm+1]) , StringToValue((string)row[indexValueHelm+2]) , StringToValue((string)row[indexValueHelm+3]) , StringToValue((string)row[indexValueHelm+4]) };

                    tokenGroup.Add(new Raider((string)row[indexName], StringToClass((string)row[indexClass]), StringToRole((string)row[indexRole]), new Status[] { StringToStatus((string)row[indexGearHelm]), StringToStatus((string)row[indexGearHelm+1]), StringToStatus((string)row[indexGearHelm+2]), StringToStatus((string)row[indexGearHelm+3]), StringToStatus((string)row[indexGearHelm+4]) }, new float[] { float.Parse(((string)row[indexTier2p]).TrimEnd('%'), NumberStyles.Float, CultureInfo.InvariantCulture), float.Parse(((string)row[indexTier4p]).TrimEnd('%'), NumberStyles.Float, CultureInfo.InvariantCulture) }, (string)row[indexOmni] == "TRUE" ? true : false, float.TryParse(((string)row[indexVault]).TrimEnd('%'), NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedValue) ? parsedValue : 0, itemValues));
                }


                //READ LOOT
                if (findingItems && row[k] != "")
                {
                    //Calculator.loot.Add(new Item(StringToSlot((string)row[k]), StringToStatus((string)row[k + 2])));
                    newLoot[StringToToken((string)row[k+1])].Add(new Item(StringToSlot((string)row[k]), StringToStatus((string)row[k + 2])));
                }

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

            return (raid,newLoot);
        }

        private static int StringToToken(string str)
        {
            switch (str)
            {
                case "Dreadful": return 0;
                case "Mystic": return 1;
                case "Venerated": return 2;
                case "Zenith": return 3;
                case "Omni": return 4;
            }
            return 0;
        }

        private static float StringToValue(string str) //Item value modifiers
        {
            switch( str)
            {
                case "Giga": return 1f;
                case "Good": return 0.7f;
                case "mid": return 0.5f;
                case "meh": return 0.3f;
                case "blacklist": return 0f;
            }
            return 0f;
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
                case "Omni": return Slot.Omni;
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
                case "Avail in vault HC": return Status.HeroicVault;
                case "Heroic": return Status.Heroic;
                case "Avail in vault M": return Status.MythicVault;
                case "Mythic": return Status.Mythic;
            }

            return Status.Empty;
        }

        private static Class StringToClass(string str)
        {
            switch (str)
            {
                case "Warrior": return Class.Warrior;
                case "Paladin": return Class.Paladin;
                case "Hunter": return Class.Hunter;
                case "Rogue": return Class.Rogue;
                case "Priest": return Class.Priest;
                case "DeathKnight": return Class.DeathKnight;
                case "Shaman": return Class.Shaman;
                case "Mage": return Class.Mage;
                case "Warlock": return Class.Warlock;
                case "Monk": return Class.Monk;
                case "Druid": return Class.Druid;
                case "DemonHunter": return Class.DemonHunter;
                case "Evoker": return Class.Evoker;
            }

            return Class.Warrior;
        }

        private static Role StringToRole(string str)
        {
            switch (str)
            {
                case "Damage": return Role.Damage;
                case "Tank": return Role.Tank;
                case "Healer": return Role.Healer;
            }

            return Role.Damage;
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
