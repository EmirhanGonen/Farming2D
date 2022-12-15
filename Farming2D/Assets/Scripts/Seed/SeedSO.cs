using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects / SeedSo")]
public class SeedSO : ScriptableObject
{
    public Crop Crop;
    public float PlantDuration;
    public int saplingIndex;
}