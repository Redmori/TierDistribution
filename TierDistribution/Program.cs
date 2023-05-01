// See https://aka.ms/new-console-template for more information
using TierDistribution;

//READ SHEET TO OBTAIN THE RAID AND LOOT
(List<Raider>[] raid, List<Item>[] loot) = Input.ReadSheet();

//PRINT THE CURRENT RAID TO CONSOLE
Output.ToConsole(raid);

//GENERATE LOOT DISTRIBUTIONS
List<Distribution>[] distributions = Calculator.GiveItems(raid, loot);

//CALCULATE THE DISTRIBUTIONS
Calculator.CalculateDistributions(raid,loot,distributions);

//PICK THE BEST DISTRIBUTION(S)
Distribution[] maxDistr = Calculator.PickBest(distributions);

//GIVE THE BEST DISTRIBUTION TO THE RAID
Calculator.DistributeLoot(loot, maxDistr);
//PRINT THE RAID TO CONSOLE WITH THEIR NEW ITEMS
Output.ToConsole(raid);

//PRINT THE ITEM TRADING TO CONSOLE
Console.WriteLine(Calculator.DistributionToString(loot, maxDistr));
