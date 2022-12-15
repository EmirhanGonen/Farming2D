using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Vector3 PlayerPosition;

    public InventoryData inventoryData;
    public List<CropData> CropDatas = new();
    public List<FieldData> FieldDatas = new();
}