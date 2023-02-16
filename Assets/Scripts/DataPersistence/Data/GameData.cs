using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : MonoBehaviour
{
    public float playTime;
    public int featherCount;
    public GameData()
    {
        this.playTime = 0f;
        featherCount = 0;
    }

}
