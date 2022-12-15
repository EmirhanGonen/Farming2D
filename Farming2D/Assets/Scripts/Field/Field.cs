using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isFull;

    public void PlantCrop(GameObject _crop)
    {
        if (isFull) return;

        Inventory.Instance.SetInventoryQuantity(Inventory.Instance.selectedSlotIndex, -1);
        Inventory.Instance.CheckCurrentSlot(); // Yada CheckAllSlot at;

        Crop crop = Instantiate(_crop, transform.position, Quaternion.identity).GetComponent<Crop>();

        crop.field = this;

        isFull = true;
    }

    public FieldData Data()
    {
        FieldData data = new()
        {
            _isFull = isFull,
            position = transform.position,
        };

        return data;
    }

    public void Load(FieldData data)
    {
        isFull = data._isFull;
        transform.position = data.position;
    }
}