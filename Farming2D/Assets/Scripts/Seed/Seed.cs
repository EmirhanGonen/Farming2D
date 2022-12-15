using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] private SeedSO so;

    public float PlantDuration => so.PlantDuration;
    public Crop Crop => so.Crop;
    public int SaplingIndex => so.saplingIndex;

}