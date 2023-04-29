// See https://aka.ms/new-console-template for more information
using TierDistribution;


//Calculator.raiders.Add(new Raider("Nox", Class.Monk, Role.Tank, new Status[5]           { Status.Empty, Status.Normal, Status.Empty, Status.Empty, Status.Empty }, new float[2]     { 1f, 2f }));
//Calculator.raiders.Add(new Raider("Kratos", Class.Warrior, Role.Damage, new Status[5]   { Status.Heroic, Status.LFR, Status.Heroic, Status.Empty, Status.Empty }, new float[2]      {3f, 4f }));
//Calculator.raiders.Add(new Raider("Fystin", Class.Warrior, Role.Damage, new Status[5]   { Status.Empty, Status.Empty, Status.Mythic, Status.Empty, Status.Empty }, new float[2]     { 5f, 6f }));
//Calculator.raiders.Add(new Raider("Push", Class.Rogue, Role.Damage, new Status[5]       { Status.Mythic, Status.Empty, Status.Empty, Status.Empty, Status.Mythic }, new float[2]    { 7f, 8f }));
//Calculator.raiders.Add(new Raider("Stealthi", Class.Rogue, Role.Damage, new Status[5]   { Status.Empty, Status.Empty, Status.Empty, Status.Empty, Status.Empty }, new float[2]      { 9f, 10f }));
//Calculator.raiders.Add(new Raider("Wolfscale", Class.Evoker, Role.Healer, new Status[5] { Status.Empty, Status.Empty, Status.LFR, Status.LFR, Status.Empty }, new float[2]          { 11f, 12f }));
//Calculator.raiders.Add(new Raider("Alece", Class.Evoker, Role.Damage, new Status[5]     { Status.Empty, Status.Heroic, Status.Normal, Status.Heroic, Status.Heroic }, new float[2]  { 13f, 14f }));


//Calculator.loot.Add(new Item(Slot.Gloves, Status.Heroic));
//Calculator.loot.Add(new Item(Slot.Shoulder, Status.Heroic));
//Calculator.loot.Add(new Item(Slot.Legs, Status.Heroic));
//Calculator.loot.Add(new Item(Slot.Shoulder, Status.Normal));
//Calculator.loot.Add(new Item(Slot.Chest, Status.Normal));
//Calculator.loot.Add(new Item(Slot.Shoulder, Status.Normal));
//Calculator.loot.Add(new Item(Slot.Shoulder, Status.Normal));
//Calculator.loot.Add(new Item(Slot.Helm, Status.Normal));


//Output.ToConsole(Calculator.raiders);

(List<Raider>[] raid, List<Item>[] loot) = Input.ReadSheet();
//Calculator.raiders = raid[3];

Output.ToConsole(raid);

List<Raider>[] maxDistr = Calculator.GiveItems(raid, loot);

Calculator.DistributeLoot(loot, maxDistr);
Output.ToConsole(raid);

//Console.WriteLine("Best match found: " + Calculator.highest);
Console.WriteLine(Calculator.DistributionToString(loot, maxDistr));
