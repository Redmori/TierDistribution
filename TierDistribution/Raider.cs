using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TierDistribution
{    public enum Status
    {
        Empty,
        LFR,
        Normal,
        Heroic,
        Mythic,
    }

    public enum Class
    {
        Warrior,
        Paladin,
        Hunter,
        Rogue,
        Priest,
        Shaman,
        Mage,
        Warlock,
        Monk,
        Druid,
        DemonHunter,
        DeathKnight,
        Evoker
    }

    public enum Role
    {
        Tank,
        Healer,
        Damage
    }
    public class Raider
    {
        public string name;
        public Class clas;
        public Role role;
        public Status[] gear;
        public int numberOfTier;

        public float[] tierValue;

        public List<Item> loot = new List<Item>();

        public Raider(string name_, Class class_, Role role_, Status[] gear_, float[] tierValue_)
        {
            name = name_;
            clas = class_;
            role = role_;
            gear = gear_;
            tierValue = tierValue_;

            numberOfTier = CalculateNumberOfTier();
        }

        public void GiveItem(Item item)
        {
            loot.Add(item);
        }

        public int CalculateNumberOfTier()
        {
            int n = 0;
            for(int  i = 0; i < 5; i++)
            {
                if (gear[i] == Status.LFR || gear[i] == Status.Normal || gear[i] == Status.Heroic || gear[i] == Status.Mythic )
                    n++;
            }
            return n;
        }

        public float CalculatePower()
        {
            if (numberOfTier >= 4) return 0f;

            int n = 0;
            Status[] newGear = new Status[5];
            Array.Copy(gear, newGear, 5);
            foreach(Item item in loot)
            {
                if (newGear[(int)item.slot] == Status.Empty)
                {
                    newGear[(int)item.slot] = item.status;
                    n++;
                }
            }
            //Console.WriteLine(name + " : " + loot.Count + " : " + numberOfTier + " + " + n);

            float result = 0.0f;
            int sum = numberOfTier + n;

            if (numberOfTier < 2 && sum >= 2)
                result += tierValue[0];

            if (numberOfTier < 4 && sum >= 4)
                result += tierValue[1];

            return result;
        }

        public override string ToString()
        {
            return $"{name}\n" +
                $"Helm:     {gear[(int)Slot.Helm]}\n" +
                $"Shoulder: {gear[(int)Slot.Shoulder]}\n" +
                $"Chest:    {gear[(int)Slot.Chest]}\n" +
                $"Gloves:   {gear[(int)Slot.Gloves]}\n" +
                $"Legs:     {gear[(int)Slot.Legs]}";
        }
    }

    public enum Slot
    {
       Helm,
       Shoulder,
       Chest,
       Gloves,
       Legs
    }
    public class Item
    {
        public Slot slot;
        public Status status;

        public Item(Slot slot_, Status status_) 
        {
            slot  = slot_;
            status = status_;
        }

    }
}
