// See https://aka.ms/new-console-template for more information
using TierDistribution;

//READ SHEET TO OBTAIN THE RAID AND LOOT
(List<Raider>[] raid, List<Item>[] loot) = Input.ReadSheet();

//PRINT THE CURRENT RAID TO CONSOLE
Output.ToConsole(raid);

int[] nToken = new int[] { 4, 5, 6, 7, 22 };
int[] nLoot = new int[] { 5, 6, 7, 8, 3 };

Chromosome ch = new Chromosome(nToken, nLoot);
Console.WriteLine(ch.ToString());




////GENERATE LOOT DISTRIBUTIONS
//List<Distribution>[] distributions = Calculator.GiveItems(raid, loot);

////CALCULATE THE DISTRIBUTIONS
//Calculator.CalculateDistributions(raid,loot,distributions);

////PICK THE BEST DISTRIBUTION(S)
//Distribution[] maxDistr = Calculator.PickBest(distributions);

////GIVE THE BEST DISTRIBUTION TO THE RAID
//Calculator.DistributeLoot(loot, maxDistr);
////PRINT THE RAID TO CONSOLE WITH THEIR NEW ITEMS
//Output.ToConsole(raid);

////PRINT THE ITEM TRADING TO CONSOLE
//Console.WriteLine(Calculator.DistributionToString(loot, maxDistr));
