using UnityEngine;


public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance { get; private set; }

    [SerializeField] private Transform TopLeftCorner;
    [SerializeField] private Transform BottomRightCorner;

    [Space(10f)]

    [SerializeField] private float yDistance;
    [SerializeField] private float xDistance;

    [Space(10f)]

    [SerializeField] private GameObject digableArePrefab;
    public Transform digableAreaParent;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CreatePieces();
    }
    private void CreatePieces()
    {
        int xAmount = (int)((BottomRightCorner.position.x - TopLeftCorner.position.x) / xDistance);
        int zAmount = (int)((TopLeftCorner.position.y - BottomRightCorner.position.y) / yDistance);

        xAmount++;
        zAmount++;

        for (int i = 0; i < xAmount; i++)
        {
            for (int j = 0; j < zAmount; j++)
            {
                GameObject grid = Instantiate(digableArePrefab);

                grid.transform.position = new Vector3(i * xDistance, j * -yDistance, 0.1f) + TopLeftCorner.position;
                grid.transform.SetParent(digableAreaParent);
            }
        }
    }
}