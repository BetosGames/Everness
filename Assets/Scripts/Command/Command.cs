using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class Command
{
    public string commandID;

    //Minimum permission level the player needs to have to run this command
    public int minPermissionLevel = 0;

    public const string NO_PERMISSION = "add4817f-6a61-4513-9d87-42b4f65c3d53";
    public const string INVALID_SYNTAX = "9da9e244-378a-46e5-8e31-fd5ae0702d28";

    public Command()
    {

    }

    public string Run(Player player, string[] args)
    {
        if(player.permissionLevel < minPermissionLevel)
        {
            return NO_PERMISSION;
        }
        else
        {
            return OnRun(player, args);
        }
    }

    public virtual string OnRun(Player player, string[] args)
    {
        return "Command Success!";
    }

    public static bool ArgsToCoords(Player player, bool tileCoords, string[] coordArgs, out Vector2 vector2Out)
    {
        float[] coords = new float[2];
        vector2Out = new Vector2(0, 0);

        if (coordArgs.Length != 2) return false;

        for (int i = 0; i < coordArgs.Length; i++)
        {
            if (coordArgs[i].StartsWith('~'))
            {
                coordArgs[i] = coordArgs[i].TrimStart('~');

                if (coordArgs[i].Length == 0)
                {
                    coords[i] = tileCoords ? player.GetTileCoordinates()[i] : player.GetCoordinates()[i];
                }
                else
                {
                    if (!float.TryParse(coordArgs[i], out coords[i]))
                    {
                        
                        return false;
                    }
                    else
                    {
                        coords[i] += tileCoords ? player.GetTileCoordinates()[i] : player.GetCoordinates()[i];
                    }
                }
            }
            else
            {
                if (!float.TryParse(coordArgs[i], out coords[i]))
                {
                    return false;
                }
            }
        }

        vector2Out = new Vector2(coords[0], coords[1]);
        return true;
    }
}
