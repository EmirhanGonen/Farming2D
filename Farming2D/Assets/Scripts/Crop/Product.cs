using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour, ITakeable
{
    public int Quantity = 1;
    public GameObject Prefab;

    public void TakeItToInventory() => Inventory.Instance.AddToInventory(Prefab);

}
public interface ITakeable 
{
    void TakeItToInventory();
}