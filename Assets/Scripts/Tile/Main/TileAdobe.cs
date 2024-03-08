using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAdobe : TileMain, IMinable
{
    public TileAdobe()
    {
        tileID = "adobe";
        coreID = 14;
        displayName = "Adobe";
        generatesWithPermaback = new TileAdobeCave();
    }

    public float mineTime => 1;

    public void OnMineTile(Player player)
    {

    }
}
