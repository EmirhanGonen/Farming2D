using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : NPC
{
    [SerializeField] private GameObject QuestionPanel;
    [SerializeField] private GameObject marketMenu;

    [SerializeField] private List<ItemSO> m_Items;
    [SerializeField] private RectTransform parent;
    [SerializeField] private GameObject m_ShopPrefab;
    [SerializeField] private float xDistance;

    // Distance per panel + 600; for x

    private void Start()
    {
        playerSprite = GameObject.FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sprite;
        CreateShop();
    }

    public void BuyObject(ItemSO itemSO)
    {
        if (Inventory.Instance.money < itemSO.cost) return;

        Debug.Log("Alýndý");

        Inventory.Instance.money -= itemSO.cost;

        Inventory.Instance.SetMoneyText();

        Inventory.Instance.AddToInventory(itemSO.product);
    }

    private void CreateShop()
    {
        List<GameObject> spawnedShopTemplate = new();
        foreach (ItemSO itemSO in m_Items)
        {
            GameObject spawnedPrefab = Instantiate(m_ShopPrefab);
            spawnedShopTemplate.Add(spawnedPrefab);
            spawnedPrefab.GetComponent<Button>().onClick.AddListener(() => { BuyObject(m_Items[m_Items.IndexOf(itemSO)]); });
            for (int i = 0; i < spawnedPrefab.transform.GetChild(0).childCount; i++)
            {
                GameObject obje = spawnedPrefab.transform.GetChild(0).GetChild(i).gameObject;

                switch (i)
                {
                    case 0: SetText(obje.GetComponent<TextMeshProUGUI>(), itemSO.ItemName); break;
                    case 1: SetText(obje.GetComponent<TextMeshProUGUI>(), itemSO.ItemDescription); break;
                    case 2: obje.GetComponent<Image>().sprite = itemSO.ItemImage; break;
                    case 3: SetText(obje.GetComponent<TextMeshProUGUI>(), "Grow Up Time : " + itemSO.growUpTime.ToString()); break;
                    case 4: SetText(obje.GetComponent<TextMeshProUGUI>(), "Plant Duration : " + itemSO.plantDuration.ToString()); break;
                    case 5: SetText(obje.GetComponent<TextMeshProUGUI>(), "Cost : " + itemSO.cost.ToString()); break;
                }
            }
        }
        for (int i = 0; i < spawnedShopTemplate.Count; i++)
        {
            spawnedShopTemplate[i].transform.SetParent(parent);
            spawnedShopTemplate[i].transform.localPosition = new((350 + (i * xDistance)) - 960, 0, 0);
        }
    }

    private void SetText(TextMeshProUGUI textMesh, string text)
    {
        if (text.Equals(string.Empty))
        {
            textMesh.gameObject.SetActive(false);
            return;
        }

        textMesh.SetText(text);
    }

    public void GetItem()
    {
        Inventory.Instance.AddToInventory(PlantManager.Instance.dirt);
    }

    private void StartDialogue()
    {
        panel.SetActive(true);
        StartCoroutine(nameof(Dialogue));
    }
    public void CloseShop() => marketMenu.SetActive(false);
    private void Update()
    {
        DialogueManage();
    }


    protected override IEnumerator Dialogue()
    {
        return base.Dialogue();
    }
    protected override void LeaveDialogue(GameObject dialoguePanel)
    {
        if (currentDialogueIndex == npc.npcDialogues.Length - 1)
        {
            marketMenu.SetActive(true);
        }
        base.LeaveDialogue(dialoguePanel);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CollisionIsPlayer(collision.gameObject)) StartDialogue();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollisionIsPlayer(collision.gameObject)) LeaveDialogue(panel);
    }
    private bool CollisionIsPlayer(GameObject collision) => collision.GetComponent<Player>() != null;
}