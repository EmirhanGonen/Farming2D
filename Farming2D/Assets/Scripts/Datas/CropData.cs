using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CropData
{
    public float elapsedTime;
    public int Stage;
    public bool isGrown;
    public CropSO cropSO;
    public int fieldIndex;
}