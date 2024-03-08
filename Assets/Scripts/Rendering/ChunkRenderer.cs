using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class ChunkRenderer : MonoBehaviour
{
    public Tilemap foreTilemap;
    public Tilemap mainTilemap;
    public Tilemap backTilemap;
    public Tilemap permabackTilemap;
}
