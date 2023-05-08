﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TierDistribution
{    
    public enum Status
    {
        Empty,
        LFR,
        Normal,
        HeroicVault,
        Heroic,
        MythicVault,
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
        public float baseFitness;
        public Status[] newGear;
        public int nOmni; //Sak omni tokens
        public bool hasOmni; //AotC omni
        public bool usedOmni = false;
        public bool hasVaultToken = false;
        public bool usedVault = false;

        public int numberOfTier;
        public int numberOfTierWithLoot;

        public float[] tierValue;
        public float[] itemValues;

        public float vaultValue;

        public float gearMultiplier;

        public List<Item> loot = new List<Item>();

        public Raider(string name_, Class class_, Role role_, Status[] gear_, float[] tierValue_, bool aotcAvail, float vaultValue_, float[] itemValues_)
        {
            name = name_;
            clas = class_;
            role = role_;
            gear = gear_;
            hasOmni = aotcAvail;
            vaultValue = vaultValue_;
            itemValues = itemValues_;

            Console.WriteLine(itemValues[0]);
            
            newGear = new Status[gear.Length];
            Array.Copy(gear, newGear, gear.Length);
            nOmni = 0;
            tierValue = tierValue_;
            tierValue[1] -= tierValue[0]; //clause that the column is 2p+4p

            if (role == Role.Healer)
                gearMultiplier = 0.8f;
            else if (role == Role.Tank)
                gearMultiplier = 0.6f;
            else
                gearMultiplier = 1f;

            for(int i = 0; i < newGear.Length; i++)
            {
                if (newGear[i] == Status.HeroicVault || newGear[i] == Status.MythicVault)
                {
                    hasVaultToken = true;
                }
            }
               
            numberOfTier = CalculateNumberOfTier();

            baseFitness = CalcFitness();
        }

        public void GiveItem(Item item)
        {
            loot.Add(item);
        }

        public void AssignItem(Item item)
        {
            //TODO check if its an upgrade?
            newGear[(int)item.slot] = item.status;
        }

        public void AssignOmni()
        {
            nOmni++;
        }

        public void ResetGear()
        {
            for (int i = 0; i < gear.Length; i++)
            {
                newGear[i] = gear[i];
            }
            nOmni = 0;
            usedOmni = false;
        }

        public float CalcFitness()
        {
            float sum = 0;
            int n = CalculateNumberOfTier();
            int assignedTier = n + nOmni;
            int finalTier = assignedTier;

            if (assignedTier >= 2)
                sum += tierValue[0];
            if (assignedTier >= 4)
                sum += tierValue[1];

            //Handle the AotC omni token (for now we just use it when we can reach 2 or 4 set. Where we only use it to get 2p if 2p is 3x better than 4p
            if (hasOmni)
            {
                if (assignedTier == 1 && tierValue[0] > 3 * tierValue[1])
                {
                    finalTier++;
                    sum += tierValue[0];
                    usedOmni = true;
                }
                else if (assignedTier == 3)
                {
                    finalTier++;
                    sum += tierValue[1];
                    usedOmni = true;
                }
            }
                ////}else if (hasVaultToken) //has an item in the vault, but no aotc token. We use vault token if its 2x better than the alternative
                ////{
                ////    if(assignedTier == 1 && tierValue[0] > 2 * vaultValue)
                ////    {
                ////        sum += tierValue[0];
                ////        usedVault = true;
                ////    }
                ////    else if (assignedTier == 3 && tierValue[1] > 2 * vaultValue)
                ////    {
                ////        sum += tierValue[1];
                ////        usedVault = true;
                ////    }
                ////}
                ////else if(hasOmni && hasVaultToken) //has both item in vault and aotc token
                ////{

                ////}
                ///

            //    if (assignedTier == 1 || assignedTier == 3)
            //{
            //    float added = 0;
                
            //}

            ////bonus if you used the alternative item in vault
            //if (!usedVault) sum += vaultValue;

            //bonus if you keep AotC omni token (for now we give half the value of upcoming tier sets)
            if(hasOmni && !usedOmni)
                sum += Math.Max(assignedTier < 2 ? tierValue[0] * 0.5f : 0f, assignedTier < 4 ? tierValue[1] * 0.5f : 0f);

            //Add some bonus for each item based on multipliers, TODO: change % to absolute (1% -> 1k)
            for(int i = 0; i < itemValues.Length; i++)
            {
                if (newGear[i] == Status.Heroic)
                    sum += 1 * itemValues[i]; 
                else if (newGear[i] == Status.Mythic)
                    sum += 2 * itemValues[i];
            }

            float progMod = 0f; //Progression modifier towards tier set
            if (finalTier == 1)
                sum += progMod * tierValue[0];
            if (finalTier == 3)
                sum += progMod * tierValue[1];

            return sum * gearMultiplier;
        }

        //public float NextTierValue(int aTier)
        //{
        //    if(aTier < 2)
        //        return tierValue[0];
        //    if(aTier < 4)
        //        return tierValue[1];
        //    return -100f;
        //}

        public int CalculateNumberOfTier()
        {
            int n = 0;
            for(int  i = 0; i < 5; i++)
            {
                if (newGear[i] == Status.LFR || newGear[i] == Status.Normal || newGear[i] == Status.Heroic || newGear[i] == Status.Mythic )
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
            numberOfTierWithLoot = numberOfTier + n;

            if (numberOfTier < 2 && numberOfTierWithLoot >= 2)
                result += tierValue[0];

            if (numberOfTier < 4 && numberOfTierWithLoot >= 4)
                result += tierValue[1];


            //TODO: compare vault options

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
       Legs,
       Omni
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
