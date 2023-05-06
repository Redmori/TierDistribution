using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using System.Reflection.Metadata.Ecma335;

namespace TierDistribution
{
    public static class Output
    {
        public static ConsoleColor GetConsoleColor(Status status)
        {            
            switch(status)
            {
                case Status.Normal: return ConsoleColor.Green;
                case Status.Heroic: return ConsoleColor.DarkMagenta;
                case Status.Mythic: return ConsoleColor.DarkYellow;
            }
            return ConsoleColor.White;
        }

        public static ConsoleColor GetConsoleColor(Class clas)
        {            
            switch (clas)
            {
                case Class.Warrior: return ConsoleColor.DarkYellow;
                case Class.Paladin: return ConsoleColor.Magenta;
                case Class.Hunter: return ConsoleColor.Green;
                case Class.Rogue: return ConsoleColor.Yellow;
                case Class.Priest: return ConsoleColor.White;
                case Class.Shaman: return ConsoleColor.DarkBlue;
                case Class.Mage: return ConsoleColor.Blue;
                case Class.Warlock: return ConsoleColor.DarkMagenta;
                case Class.Monk: return ConsoleColor.Cyan;
                case Class.Druid: return ConsoleColor.DarkYellow;
                case Class.DemonHunter: return ConsoleColor.DarkMagenta;
                case Class.DeathKnight: return ConsoleColor.DarkRed;
                case Class.Evoker: return ConsoleColor.Cyan;
            }
            return ConsoleColor.White;
        }

        public static void ToConsole(List<Raider>[] raiders)
        {
            Console.WriteLine();
            Console.WriteLine("Name".PadRight(10) + "Helm".PadRight(10) + "Shoulder".PadRight(10) + "Chest".PadRight(10) + "Gloves".PadRight(10) + "Legs");
            for (int i = 0; i < raiders.Length; i++)
            {
                RaidersToConsole(raiders[i]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void ToConsole(List<Raider> raiders)
        {
            Console.WriteLine();
            Console.WriteLine("Name".PadRight(10) + "Helm".PadRight(10) + "Shoulder".PadRight(10) + "Chest".PadRight(10) + "Gloves".PadRight(10) + "Legs");
            RaidersToConsole(raiders);

            Console.WriteLine();

        }

        public static void RaidersToConsole(List<Raider> raiders)
        {


            foreach (Raider raider in raiders)
            {
                Console.ForegroundColor = GetConsoleColor(raider.clas);
                Console.Write(raider.name.PadRight(10));

                //General gear
                for (int i = 0; i < 5; i++)
                {
                    if (raider.newGear[i] != raider.gear[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = GetConsoleColor(raider.newGear[i]);
                        Console.Write(raider.newGear[i].ToString().PadRight(10));
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = GetConsoleColor(raider.newGear[i]);
                        Console.Write(raider.newGear[i] == Status.Empty ? "-".PadRight(10) : raider.newGear[i].ToString().PadRight(10));
                    }
                }

                //Omni token
                if(raider.nOmni > 0)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(raider.nOmni.ToString().PadRight(2).PadLeft(3));
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.Write("".PadRight(2).PadLeft(3));
                }

                //AotC token
                if (raider.usedOmni)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("o".PadRight(2).PadLeft(3));
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.Write("".PadRight(2).PadLeft(3));
                }

                //Tier values
                int nTier = raider.CalculateNumberOfTier() + raider.nOmni + (raider.usedOmni ? 1 : 0);
                if (nTier >= 2 && raider.numberOfTier < 2)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (nTier >= 2)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write((" " + (Math.Round(raider.tierValue[0],2).ToString())).PadRight(5));
                if (nTier >= 4 && raider.numberOfTier < 4)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (nTier >= 4)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write((" " + (Math.Round(raider.tierValue[1],2).ToString())).PadRight(5));

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(" ");
            }


        }
        
        public static void WriteToSheet() //DOESNT WORK
        {
            var sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyAIpz0Rm9ywnetyh76B49uEesbo1jYQk6Y"
            });

            var spreadsheetId = "14fm2C7bpPJ7EzGTBdYFXHqL7KpSHGCpf8ktuNgLpn9o";
            var sheetName = "test";
            var range = "test!G1:H2";

            // create the values object to hold the new data
            var values = new List<IList<object>>
            {
                new List<object> {"Value 1", "Value 2"},
                new List<object> {"Value 3", "Value 4"}
            };

            // create the request to update the data
            var updateRequest = new ValueRange { Values = values };
            var update = sheetsService.Spreadsheets.Values.Update(updateRequest, spreadsheetId, range);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var result = update.Execute();
        }

        public static void WriteToSheet2() //DOESNT WORK
        {
            var credential = GoogleCredential.FromFile("client_secret_882081320166-teh8os6ppeli8drtrjseo448a4c9mf0v.apps.googleusercontent.com.json").CreateScoped(new[] { SheetsService.Scope.Spreadsheets });

            // Create the Sheets API service using the credentials.
            var sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Desktop app"
            });
        }

        //public static void ToConsole(List<Raider> raiders)
        //{
        //    Console.WriteLine("Name".PadRight(10) + "Helm".PadRight(10) + "Shoulder".PadRight(10) + "Chest".PadRight(10) + "Gloves".PadRight(10) + "Legs");
        //    foreach (Raider raider in raiders)
        //    {
        //        Console.ForegroundColor = GetConsoleColor(raider.clas);
        //        Console.Write(raider.name.PadRight(10));
        //        Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Helm]); 
        //        Console.Write(raider.gear[(int)Slot.Helm] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Helm].ToString().PadRight(10));
        //        Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Shoulder]); 
        //        Console.Write(raider.gear[(int)Slot.Shoulder] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Shoulder].ToString().PadRight(10));
        //        Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Chest]);
        //        Console.Write(raider.gear[(int)Slot.Chest] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Chest].ToString().PadRight(10));
        //        Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Gloves]);
        //        Console.Write(raider.gear[(int)Slot.Gloves] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Gloves].ToString().PadRight(10));
        //        Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Legs]);
        //        Console.Write(raider.gear[(int)Slot.Legs] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Legs].ToString().PadRight(10));
        //        Console.ForegroundColor = ConsoleColor.White;
        //        Console.Write(raider.tierValue[0].ToString().PadRight(10));
        //        Console.Write(raider.tierValue[1].ToString().PadRight(10));
        //        Console.WriteLine();
        //    }

        //    Console.ForegroundColor = ConsoleColor.White;
        //}

    }
}
