// See https://aka.ms/new-console-template for more information
using TierDistribution;

//READ SHEET TO OBTAIN THE RAID AND LOOT
(List<Raider>[] raid, List<Item>[] loot) = Input.ReadSheet();

//PRINT THE CURRENT RAID TO CONSOLE
Output.ToConsole(raid);

//GIVE THE LOOT RECURSIVELY TO THE RAID AND FIND THE MAXIMUM DISTRIBUTION
List<Raider>[] maxDistr = Calculator.GiveItems(raid, loot);

//GIVE THE BEST DISTRIBUTION TO THE RAID
Calculator.DistributeLoot(loot, maxDistr);
//PRINT THE RAID TO CONSOLE WITH THEIR NEW ITEMS
Output.ToConsole(raid);

//PRINT THE ITEM TRADING TO CONSOLE
Console.WriteLine(Calculator.DistributionToString(loot, maxDistr));
