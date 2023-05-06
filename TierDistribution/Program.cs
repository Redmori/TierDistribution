// See https://aka.ms/new-console-template for more information
using TierDistribution;

//READ SHEET TO OBTAIN THE RAID AND LOOT
(List<Raider>[] raid, List<Item>[] loot) = Input.ReadSheet();
int nOmni = 3;
//PRINT THE CURRENT RAID TO CONSOLE
Output.ToConsole(raid);

int[] nToken = new int[] { raid[0].Count, raid[1].Count, raid[2].Count, raid[3].Count, raid[0].Count + raid[1].Count + raid[2].Count + raid[3].Count };
int[] nLoot = new int[] { loot[0].Count, loot[1].Count, loot[2].Count, loot[3].Count, nOmni };

//Chromosome ch = new Chromosome(nToken, nLoot);
//Console.WriteLine(ch.ToString());
//Console.WriteLine();
//Chromosome ch2 = new Chromosome(nToken, nLoot);
//Console.WriteLine(ch2.ToString());
//Console.WriteLine();
//Chromosome child = ch.Mutate(ch2, nToken, nLoot);
//Console.WriteLine(child.ToString());

int populationSize = 500;
List<Chromosome> population = new List<Chromosome>();

for(int i = 0; i < populationSize; i++)
{
    population.Add(new Chromosome(nToken, nLoot));
}

int nGenerations = 2000;
int nLoading = (int)((float)nGenerations * 0.1);
int nElites = (int)Math.Round(populationSize * 0.01);
int nMutations = (int)Math.Round(populationSize * 0.5);

for (int i = 0; i < nGenerations; i++)
{
    if (i % nLoading == 0)
        Console.Write(".");

    foreach(Chromosome chromosome in population)
        chromosome.CalcNewFitness(raid,loot,nOmni);
    
    population = population.OrderByDescending(c => c.fitness).ToList();

    if (population[0].fitness > 200)
    {
        Console.WriteLine("perfect fitness found after " + i + " generations.");
        break;
    }

    //Console.WriteLine(population[0].fitness);

    List<Chromosome> newPopulation = population.Take(nElites).ToList();

    Random random = new Random();
    while(newPopulation.Count < populationSize)
    {
        int r1 = random.Next(nMutations);
        int r2 = random.Next(nMutations);

        newPopulation.Add(population[r1].Mutate(population[r2], nToken, nLoot));
    }

    population = newPopulation;
}
Console.WriteLine("Solution found with fitness: " + population[0].fitness);
population[0].CalcNewFitness(raid,loot, nOmni);
//Console.WriteLine(population[0].ToString());
Output.ToConsole(raid);

//Console.WriteLine();
//Console.WriteLine(population[0].ToString());
//Console.WriteLine(population[1].ToString());




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
