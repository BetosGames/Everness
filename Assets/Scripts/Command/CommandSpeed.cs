using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandSpeed : Command
{
    public CommandSpeed()
    {
        minPermissionLevel = 0;
        commandID = "speed";
    }

    public override string OnRun(Player player, string[] args)
    {
        if(args.Length != 1)
        {
            return "<red>Proper Syntax: <yellow>/speed [value/default]";
        }
        else
        {
            float speedValue;

            if (!float.TryParse(args[0], out speedValue))
            {
                if (args[0] == "default")
                {
                    speedValue = 1;
                }
                else
                {
                    return "<red>Proper Syntax: <yellow>/speed [value/default]";
                }
            }

            player.SetSpeed(speedValue);
            return $"Set speed to {speedValue}!" + (speedValue == 1 ? " (default)" : "");
        }
    }
}
