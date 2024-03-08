using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandSpawn : Command
{
    public CommandSpawn()
    {
        minPermissionLevel = 0;
        commandID = "spawn";
    }

    public override string OnRun(Player player, string[] args)
    {
        if(args.Length != 3)
        {
            return "<red>Proper Syntax: <yellow>/spawn [entity] [xCoord] [yCoord]";
        }
        else
        {
            Vector2 toCoords = new Vector2();

            if (ArgsToCoords(player, false, args.Skip(1).ToArray(), out toCoords))
            {
                Entity spawnedEntity = Universe.INSTANCE.activePlanet.SpawnEntity(args[0], toCoords);

                if(spawnedEntity == null)
                {
                    return "<red>Not a valid entity!";
                }
                else
                {
                    return $"Spawned {spawnedEntity.displayName} at {toCoords[0]}, {toCoords[1]}!";
                }

                
            }
            else
            {
                return "<red>Proper Syntax: <yellow>/spawn [entity] [xCoord] [yCoord]";
            }
        }
    }
}
