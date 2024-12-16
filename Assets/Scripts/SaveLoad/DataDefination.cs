using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public string ID;

    private void OnValidate() {
        if (ID == string.Empty)
        {
            ID = System.Guid.NewGuid().ToString(); // FIX: 会不会每次启动游戏都会更新?
        }
    }
}