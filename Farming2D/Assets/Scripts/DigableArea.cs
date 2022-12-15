using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigableArea : MonoBehaviour
{
    public static DigableArea Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    public void Dig(GameObject dirt)
    {
        GameObject _dirt = Instantiate(dirt, transform.position, transform.localRotation);

        _dirt.transform.SetParent(PlantManager.Instance.ArableLand.transform);

        GamePersist.Instance.fields.Add(_dirt.GetComponent<Field>());

        Inventory.Instance.SetInventoryQuantity(Inventory.Instance.selectedSlotIndex, -1);

        Inventory.Instance.CheckCurrentSlot(); // Yada CheckAllSlot at;

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Field _)) gameObject.SetActive(false);
    }
}