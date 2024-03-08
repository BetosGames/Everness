using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureTree : Structure
{
    public StructureTree()
    {
        structureID = "tree";
        legend = new Dictionary<char, Tile>() { {'#', new TileWalnutLeaves() }, { 'H', new TileWalnutLog() }};
        origin = new Vector2Int(3, 1);
        lengthOfBiggestDimensionEver = 15;
    }

    public override string[] Pattern(System.Random random)
    {
        string[] treePattern = new string[7 + random.Next(3, 9)];

        treePattern[0] = $"--{L(random)}{L(random)}---";
        treePattern[1] = $"-{L(random)}##{L(random)}{L(random)}-";
        treePattern[2] = $"-{L(random)}####{L(random)}";
        treePattern[3] = $"{L(random)}#####{L(random)}";
        treePattern[4] = $"{L(random)}#####-";
        treePattern[5] = $"-##H##-";
        treePattern[6] = $"-{L(random)}-H-{L(random)}-";

        for (int i = 7; i < treePattern.Length; i++)
        {
            if(i < treePattern.Length - 1)
            {
                treePattern[i] = "---H---";
            }
            else
            {
                treePattern[i] = "-------";
            }
        }
        return treePattern;
    }

    public char L(System.Random random)
    {
        return random.Next(0, 4) == 0 ? '-' : '#';
    }

    public char T(System.Random random)
    {
        return random.Next(0, 6) == 0 ? 'H' : '#';
    }
}
