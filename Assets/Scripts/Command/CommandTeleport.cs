using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandTeleport : Command
{
    public CommandTeleport()
    {
        minPermissionLevel = 0;
        commandID = "tp";
    }

    public override string OnRun(Player player, string[] args)
    {
        if(args.Length != 2)
        {
            return "<red>Proper Syntax: <yellow>/tp [xCoord] [yCoord]";
        }
        else
        {
            Vector2 toCoords = new Vector2();

            if(ArgsToCoords(player, false, args, out toCoords))
            {
                player.GoToCoordinates(toCoords[0], toCoords[1]);
                return $"Teleported to {toCoords[0]}, {toCoords[1]}!";
            }
            else
            {
                return "<red>Proper Syntax: <yellow>/tp [xCoord] [yCoord]";
            }
        }
    }
}
