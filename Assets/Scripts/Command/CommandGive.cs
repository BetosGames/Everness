using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGive : Command
{
    public CommandGive()
    {
        minPermissionLevel = 0;
        commandID = "give";
    }

    public override string OnRun(Player player, string[] args)
    {
        if (args.Length < 2)
        {
            return "<red>Proper Syntax: <yellow>/give [item] [count]";
        }
        else
        {
            int count;

            if (int.TryParse(args[1], out count))
            {
                Item givenItem = Registry.NewItemFromID(args[0]);

                if (givenItem != null)
                {
                    GUIInventory.INSTANCE.AddItem(givenItem, count);
                    return $"Gave {count} of {givenItem.displayName} to {player.gamename}!";
                }
                else
                {
                    return "<red>Not a valid item!";
                }
            }

            else
            {
                return "<red>Proper Syntax: <yellow>/give [item] [count]";
            } 
        }
    }
}
