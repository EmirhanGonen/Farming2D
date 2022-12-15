using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    [SerializeField] private Transform tempParent;

    [SerializeField] private Transform mainParent;

    [SerializeField] private Vector3 firstPosition;

    [SerializeField] private bool isMoving;

    public int slotIndex;

    public InventorySlot collisionSlot;

    private void Awake()
    {
        SetVariables();
    }

    public void SetVariables()
    {
        mainParent = transform.parent;
        firstPosition = transform.position;
        transform.position = firstPosition;
    }

    public void DragHandler(BaseEventData data)
    {

        PointerEventData pointer = (PointerEventData)data;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        (RectTransform)canvas.transform, pointer.position, canvas.worldCamera, out Vector2 position);

        transform.position = canvas.transform.TransformPoint(position);
    }

    public void BeginDrag()
    {
        transform.SetParent(tempParent);
        isMoving = true;
    }

    public void EndDrag()
    {
        isMoving = false;
        transform.SetParent(mainParent);

        if (collisionSlot) Inventory.Instance.ChangeInventorySlots(slotIndex, collisionSlot.slotIndex);

        transform.position = firstPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out InventorySlot inventory)) return;
        if (!isMoving) return;
        collisionSlot = inventory;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out InventorySlot inventory)) return;
        if (!isMoving) return;
        collisionSlot = null;
    }
}