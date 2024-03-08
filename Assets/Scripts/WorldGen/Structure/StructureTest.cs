 using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureTest : Structure
{
    public StructureTest()
    {
        structureID = "test";
        legend = new Dictionary<char, Tile>() { {'#', null }, { 'O', null }, { 'X', null } };
        origin = new Vector2Int(0, 0);
        lengthOfBiggestDimensionEver = 5;
    }

    public override string[] Pattern(System.Random random)
    {
        int coinFlip = random.Next(0, 2);

        if(coinFlip == 0)
        {
            return new string[]
        { "-###-",
          "#OOO#",
          "#OOO#",
          "#OOO#",
          "-###-"};
        }
        else
        {
            return new string[]
        { "-XXX-",
          "XOOOX",
          "XOOOX",
          "XOOOX",
          "-XXX-"};
        }


    }
}
