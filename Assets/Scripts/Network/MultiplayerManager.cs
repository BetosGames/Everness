using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager INSTANCE;

    void Start()
    {
        INSTANCE = this;
        DontDestroyOnLoad(this);
    }
}
