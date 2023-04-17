using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                case Class.Shaman: return ConsoleColor.Blue;
                case Class.Mage: return ConsoleColor.DarkBlue;
                case Class.Warlock: return ConsoleColor.DarkMagenta;
                case Class.Monk: return ConsoleColor.Cyan;
                case Class.Druid: return ConsoleColor.DarkGreen;
                case Class.DemonHunter: return ConsoleColor.DarkMagenta;
                case Class.DeathKnight: return ConsoleColor.DarkRed;
                case Class.Evoker: return ConsoleColor.Cyan;
            }
            return ConsoleColor.White;
        }

        public static void ToConsole(List<Raider> raiders)
        {
            Console.WriteLine("Name".PadRight(10) + "Helm".PadRight(10) + "Shoulder".PadRight(10) + "Chest".PadRight(10) + "Gloves".PadRight(10) + "Legs");
            foreach (Raider raider in raiders)
            {
                Console.ForegroundColor = GetConsoleColor(raider.clas);
                Console.Write(raider.name.PadRight(10));
                Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Helm]); 
                Console.Write(raider.gear[(int)Slot.Helm] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Helm].ToString().PadRight(10));
                Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Shoulder]); 
                Console.Write(raider.gear[(int)Slot.Shoulder] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Shoulder].ToString().PadRight(10));
                Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Chest]);
                Console.Write(raider.gear[(int)Slot.Chest] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Chest].ToString().PadRight(10));
                Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Gloves]);
                Console.Write(raider.gear[(int)Slot.Gloves] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Gloves].ToString().PadRight(10));
                Console.ForegroundColor = GetConsoleColor(raider.gear[(int)Slot.Legs]);
                Console.WriteLine(raider.gear[(int)Slot.Legs] == Status.Empty ? "-".PadRight(10) : raider.gear[(int)Slot.Legs].ToString().PadRight(10));
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
