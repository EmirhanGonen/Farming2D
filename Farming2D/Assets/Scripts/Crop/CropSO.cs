using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Crop_ScriptableObject")]
public class CropSO : ScriptableObject
{
    public GameObject Output;
    public Sprite[] growStageSprite;
    public float[] growStageUpTime;
}