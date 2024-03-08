using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandJump : Command
{
    public CommandJump()
    {
        minPermissionLevel = 0;
        commandID = "jump";
    }

    public override string OnRun(Player player, string[] args)
    {
        if (args.Length != 1)
        {
            return "<red>Proper Syntax: <yellow>/jump [value/default]";
        }
        else
        {
            float jumpHeightValue;

            if (!float.TryParse(args[0], out jumpHeightValue))
            {
                if (args[0] == "default")
                {
                    jumpHeightValue = 1;
                }
                else
                {
                    return "<red>Proper Syntax: <yellow>/jump [value/default]";
                }
            }

            player.SetJumpHeight(jumpHeightValue);
            return $"Set jump to {jumpHeightValue}!" + (jumpHeightValue == 1 ? " (default)" : "");
        }
    }
}
