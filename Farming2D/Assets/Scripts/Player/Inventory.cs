using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Inventory : MonoBehaviour
{

    public static Inventory Instance { get; private set; }

    public int money;

    [SerializeField] private KeyCode InventoryOpenKey = KeyCode.E;

    [SerializeField] private GameObject InventoryPanel;

    [SerializeField] private TMP_Text moneyText;

    [SerializeField] private GameObject arableLands;

    [SerializeField] private Sprite emptySlot;

    public List<Slot> Slots = new();
    public int selectedSlotIndex;
    [SerializeField] private Transform _inventory;
    [SerializeField] private Transform _inventoryAll;

    private void Awake()
    {
        Instance = this;
        SetMoneyText();
        CheckCurrentSlot();
        SetAllInventory();
        SetSlotScale(0, selectedSlotIndex);

    }
    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0) SetSlotIndex(-1);
        if (Input.mouseScrollDelta.y < 0) SetSlotIndex(1);
        if (Input.GetKeyDown(InventoryOpenKey)) InventoryPanel.SetActive(!InventoryPanel.activeSelf);
    }

    public void AddToInventory(GameObject Item)
    {
        Slot tempSlot;
        foreach (Slot slot in Slots) //Eðer o Itemden Varsa Buraya Giriyor
        {
            if (!slot.Item) continue;
            if (Item.name != slot.Item.name) continue;
            slot._quantity++;

            Transform set = Slots.IndexOf(slot) > 2 ? _inventoryAll : _inventory;
            int slotIsInventoryShortCut = Slots.IndexOf(slot) > 2 ? 3 : 0;


            SetItemText(set.GetChild(Slots.IndexOf(slot) - slotIsInventoryShortCut).GetChild(0), slot._quantity.ToString());
            return;
        }
        for (int i = 0; i < Slots.Count; i++) // Uygun Slotu Bulup Itemi Yerleþtiriyor
        {
            if (Slots[i].Item) continue; //Uygun  Slotu Arýyor

            tempSlot = Slots[i];

            tempSlot.Item = Item;
            tempSlot._quantity = 1;

            Transform set = i > 2 ? _inventoryAll : _inventory;

            int slotIsInventoryShortCut = i > 2 ? 3 : 0;


            SetItemSprite(set.GetChild(Slots.IndexOf(tempSlot) - slotIsInventoryShortCut).GetChild(0), tempSlot.Item.GetComponent<SpriteRenderer>().sprite);
            SetItemText(set.GetChild(Slots.IndexOf(tempSlot) - slotIsInventoryShortCut).GetChild(0), tempSlot._quantity.ToString());
            break;
        }
        CheckCurrentSlot();
    }

    public void SetMoneyText() => moneyText.SetText($"Money  : {money} ");


    private void SetSlotIndex(int amount)
    {
        int oldSlotIndex = selectedSlotIndex;
        selectedSlotIndex += amount;
        selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, _inventory.childCount - 1);
        if (selectedSlotIndex < _inventory.childCount) { SetSlotScale(oldSlotIndex, selectedSlotIndex); }
        CheckCurrentSlot();
    }

    private void SetSlotScale(int oldIndex, int newIndex)
    {
        for (int i = 0; i < _inventory.childCount; i++)
        {
            if (i == newIndex) continue;
            if (_inventory.GetChild(i).transform.localScale.x != 1.00f)
            {
                _inventory.GetChild(i).transform.DOKill();
                _inventory.GetChild(i).transform.DOScale(1f, .2f);
            }
        }
        _inventory.GetChild(oldIndex).transform.DOScale(1f, .2f);
        _inventory.GetChild(newIndex).transform.DOScale(1.15f, .3f);
    }

    public void CheckCurrentSlot()
    {
        if (InTheSlotIsSeed()) PlantManager.Instance.currentSaplingIndex = InTheSlotIsSeed().SaplingIndex;
        else PlantManager.Instance.currentSaplingIndex = -1;


        arableLands.SetActive(InTheSlotIsSeed() | InTheSlotIsField());
        GridGenerator.Instance.digableAreaParent.gameObject.SetActive(InTheSlotIsField());
    }
    private Seed InTheSlotIsSeed()
    {
        if (Slots.Count - 1 < selectedSlotIndex | Slots.Count == 0 | !Slots[selectedSlotIndex].Item) return null;
        if (Slots[selectedSlotIndex].Item.TryGetComponent(out Seed seed)) return seed;
        return null;
    }
    private Field InTheSlotIsField()
    {
        if (Slots.Count - 1 < selectedSlotIndex | Slots.Count == 0 | !Slots[selectedSlotIndex].Item) return null;
        if (Slots[selectedSlotIndex].Item.TryGetComponent(out Field field)) return field;
        return null;
    }

    public void ChangeInventorySlots(int firstSlotIndex, int secondSlotIndex)
    {
        Slot firstSlot = Slots[firstSlotIndex];
        Slot secondSlot = Slots[secondSlotIndex];

        Slot tempFirstSlot = firstSlot;

        Slots[firstSlotIndex] = secondSlot;

        Transform mainFirstTransform = GetSlotTransform(firstSlotIndex).GetChild(firstSlotIndex - GetSlotIsShortCut(firstSlotIndex)).GetChild(0);

        mainFirstTransform.localPosition = Vector3.zero;

        Sprite setSprite = Slots[firstSlotIndex].Item ? Slots[firstSlotIndex].Item.GetComponent<SpriteRenderer>().sprite : emptySlot;
        string setQuantity = Slots[firstSlotIndex]._quantity > 0 ? Slots[firstSlotIndex]._quantity.ToString() : "";

        InventorySlot firstInventory = mainFirstTransform.GetComponent<InventorySlot>();
        firstInventory.collisionSlot = null;


        firstInventory.SetVariables();

        SetItemSprite(mainFirstTransform, setSprite);

        SetItemText(mainFirstTransform, setQuantity);

        mainFirstTransform.GetComponent<InventorySlot>().SetVariables();

        Slots[secondSlotIndex] = tempFirstSlot;

        Transform mainSecondTransform = GetSlotTransform(secondSlotIndex).GetChild(secondSlotIndex - GetSlotIsShortCut(secondSlotIndex)).GetChild(0);

        Sprite setSpriteSecond = Slots[secondSlotIndex].Item ? Slots[secondSlotIndex].Item.GetComponent<SpriteRenderer>().sprite : emptySlot;
        string setQuantitySecond = Slots[secondSlotIndex]._quantity > 0 ? Slots[secondSlotIndex]._quantity.ToString() : "";

        SetItemSprite(mainSecondTransform, setSpriteSecond);
        SetItemText(mainSecondTransform, setQuantitySecond);

        InventorySlot secondInventory = mainSecondTransform.GetComponent<InventorySlot>();

        secondInventory.SetVariables();
    }

    private Transform GetSlotTransform(int index) => index > 2 ? _inventoryAll : _inventory;
    private int GetSlotIsShortCut(int index) => index > 2 ? 3 : 0;


    public void SetInventoryQuantity(int slotIndex, int amount)
    {
        Slots[slotIndex]._quantity += amount;

        _inventory.GetChild(slotIndex).GetChild(0).Find("Quantity").GetComponent<TextMeshProUGUI>().SetText(Slots[slotIndex]._quantity.ToString());

        if (Slots[slotIndex]._quantity > 0) return;

        Slots.Remove(Slots[slotIndex]);

        _inventory.GetChild(slotIndex).GetChild(0).Find("Quantity").GetComponent<TextMeshProUGUI>().SetText("");
        _inventory.GetChild(slotIndex).GetChild(0).Find("ItemSprite").GetComponent<Image>().sprite = emptySlot;
        SetAllInventory();
    }

    public void SetAllInventory()
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            Transform set = i > 2 ? _inventoryAll : _inventory;

            int slotIsInventoryShortCut = i > 2 ? 3 : 0;

            Transform inventorySlot = set.GetChild(i - slotIsInventoryShortCut).GetChild(0);

            inventorySlot.GetComponent<InventorySlot>().slotIndex = i;

            inventorySlot.Find("ItemSprite").GetComponent<Image>().sprite = emptySlot;
            inventorySlot.Find("Quantity").GetComponent<TextMeshProUGUI>().SetText("");
        }
        foreach (Slot slot in Slots)
        {

            Transform set = Slots.IndexOf(slot) > 2 ? _inventoryAll : _inventory;

            int slotIsInventoryShortCut = Slots.IndexOf(slot) > 2 ? 3 : 0;

            Transform inventorySlot = set.GetChild(Slots.IndexOf(slot) - slotIsInventoryShortCut).GetChild(0);

            if (!slot.Item) continue;

            inventorySlot.Find("ItemSprite").GetComponent<Image>().sprite = slot.Item.GetComponent<SpriteRenderer>().sprite;
            inventorySlot.Find("Quantity").GetComponent<TextMeshProUGUI>().SetText(slot._quantity.ToString());
        }
    }

    private void SetItemSprite(Transform inventorySlot, Sprite sprite) => inventorySlot.Find("ItemSprite").GetComponent<Image>().sprite = sprite;
    private void SetItemText(Transform inventorySlot, string TextString) => inventorySlot.Find("Quantity").GetComponent<TextMeshProUGUI>().SetText(TextString);

    public InventoryData Data()
    {
        InventoryData data = new()
        {
            slots = Slots,
        };

        return data;
    }
    public void Load(InventoryData data)
    {
        Slots = data.slots;
        foreach (var slot in Slots)
        {
            Transform set = Slots.IndexOf(slot) > 2 ? _inventoryAll : _inventory;

            int slotIsInventoryShortCut = Slots.IndexOf(slot) > 2 ? 3 : 0; // Slot Hýzlý eriþimdemi deðilmi onu kontrol ediyor

            Transform inventory = set.GetChild(Slots.IndexOf(slot) - slotIsInventoryShortCut).GetChild(0);

            if (!slot.Item) continue;

            SetItemSprite(inventory, slot.Item.GetComponent<SpriteRenderer>().sprite);
            SetItemText(inventory, slot._quantity.ToString());
        }
        CheckCurrentSlot();
    }
}

[Serializable]
public class Slot
{
    public GameObject Item;
    public int _quantity;
}