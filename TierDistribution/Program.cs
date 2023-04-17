// See https://aka.ms/new-console-template for more information
using TierDistribution;


Calculator.raiders.Add(new Raider("Nox", Class.Monk, Role.Tank, new Status[5]           { Status.Empty, Status.Normal, Status.Empty, Status.Empty, Status.Empty }, new float[2]     { 1f, 2f }));
Calculator.raiders.Add(new Raider("Kratos", Class.Warrior, Role.Damage, new Status[5]   { Status.Heroic, Status.LFR, Status.Heroic, Status.Empty, Status.Empty }, new float[2]      {3f, 4f }));
Calculator.raiders.Add(new Raider("Fystin", Class.Warrior, Role.Damage, new Status[5]   { Status.Empty, Status.Empty, Status.Mythic, Status.Empty, Status.Empty }, new float[2]     { 5f, 6f }));
Calculator.raiders.Add(new Raider("Push", Class.Rogue, Role.Damage, new Status[5]       { Status.Mythic, Status.Empty, Status.Empty, Status.Empty, Status.Mythic }, new float[2]    { 7f, 8f }));
Calculator.raiders.Add(new Raider("Stealthi", Class.Rogue, Role.Damage, new Status[5]   { Status.Empty, Status.Empty, Status.Empty, Status.Empty, Status.Empty }, new float[2]      { 9f, 10f }));
Calculator.raiders.Add(new Raider("Wolfscale", Class.Evoker, Role.Healer, new Status[5] { Status.Empty, Status.Empty, Status.LFR, Status.LFR, Status.Empty }, new float[2]          { 11f, 12f }));
Calculator.raiders.Add(new Raider("Alece", Class.Evoker, Role.Damage, new Status[5]     { Status.Empty, Status.Heroic, Status.Normal, Status.Heroic, Status.Heroic }, new float[2]  { 13f, 14f }));


Calculator.loot.Add(new Item(Slot.Legs, Status.Heroic));
Calculator.loot.Add(new Item(Slot.Helm, Status.Heroic));
Calculator.loot.Add(new Item(Slot.Legs, Status.Heroic));
Calculator.loot.Add(new Item(Slot.Legs, Status.Normal));
Calculator.loot.Add(new Item(Slot.Chest, Status.Heroic));
Calculator.loot.Add(new Item(Slot.Chest, Status.Normal));
Calculator.loot.Add(new Item(Slot.Shoulder, Status.Normal));
Calculator.loot.Add(new Item(Slot.Chest, Status.Normal));
Calculator.loot.Add(new Item(Slot.Shoulder, Status.Normal));
Calculator.loot.Add(new Item(Slot.Shoulder, Status.Normal));
Calculator.loot.Add(new Item(Slot.Helm, Status.Normal));


Output.ToConsole(Calculator.raiders);


//Input.ReadSheet();

////GENERATE RANDOM ITEMS
//Random random = new Random();
//for (int i = 0; i < 10; i++)
//{
//    Slot slot = (Slot)random.Next(Enum.GetValues(typeof(Slot)).Length);
//    Status status = (Status)random.Next(Enum.GetValues(typeof(Status)).Length);
//    Calculator.loot.Add(new Item(slot, status));
//}

////GENERATE RANDOM RAIDERS
//random = new Random();
//for (int i = 0; i < 5; i++)
//{
//    string name = "Raider" + (i + 1).ToString();
//    Class playerClass = (Class)random.Next(Enum.GetValues(typeof(Class)).Length);
//    Role role = (Role)random.Next(Enum.GetValues(typeof(Role)).Length);
//    Status[] gear = new Status[5];
//    float[] stats = new float[2];
//    for (int j = 0; j < 5; j++)
//    {
//        if (random.NextDouble() < 0.6)
//        {
//            gear[j] = Status.Empty;
//        }
//        else
//        {
//            gear[j] = (Status)random.Next(1, Enum.GetValues(typeof(Status)).Length);
//        }
//    }
//    for (int j = 0; j < 2; j++)
//    {
//        stats[j] = (float)random.NextDouble() * 10.0f + 10.0f;
//    }
//    Calculator.raiders.Add(new Raider(name, playerClass, role, gear, stats));
//}


//foreach (Raider raider in Calculator.raiders)
//    Console.WriteLine(raider.ToString());


Calculator.GiveItems();
Console.WriteLine("Best match found: " + Calculator.highest);
Console.WriteLine(Calculator.DistributionToString(Calculator.bestDistr));
