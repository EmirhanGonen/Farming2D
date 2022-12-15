using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance { get; private set; }

    public GameObject ArableLand;

    [SerializeField] private List<Seed> Saplings = new();

     public GameObject dirt;

    [SerializeField] private GameObject progressBar;
    [SerializeField] private Image progressBarFill;

    private Camera MainCamera;

    private RaycastHit2D Hit;


    public int currentSaplingIndex;

    public bool isHandFull;



    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        MainCamera = Camera.main;
    }

    private void Update()
    {

        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);

        Hit = Physics2D.Raycast(MousePosition, Vector3.forward, 1f);

        if (!Hit.collider) return;

        if (Hit.collider.TryGetComponent(out Product product)) { product.TakeItToInventory(); Destroy(product.gameObject); }

        if (Hit.collider.TryGetComponent(out DigableArea digableArea)) digableArea.Dig(dirt);

        Hit.collider.TryGetComponent(out Field field);

        if (field & currentSaplingIndex >= 0) StartCoroutine(Plant_CO(field));
    }

    private IEnumerator Plant_CO(Field field)
    {
        if (progressBarFill.fillAmount > .00f) yield break;

        Seed seed = Saplings[currentSaplingIndex];
        progressBar.SetActive(true);

        float tempPlayerSpeed = Player.Instance.Speed;
        Player.Instance.Speed = .00f;

        while (progressBarFill.fillAmount < 1f)
        {
            progressBarFill.fillAmount += Time.deltaTime / seed.PlantDuration;
            yield return null;
        }

        progressBar.SetActive(false);

        field.PlantCrop(seed.Crop.gameObject);
        Player.Instance.Speed = tempPlayerSpeed;

        progressBarFill.fillAmount = .00f;
    }
}