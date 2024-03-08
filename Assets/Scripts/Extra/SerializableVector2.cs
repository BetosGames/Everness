using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableVector2
{
    public float x;
    public float y;

    public SerializableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
