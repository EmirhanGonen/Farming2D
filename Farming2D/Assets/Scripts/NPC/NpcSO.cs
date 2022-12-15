using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/NPC")]
public class NpcSO : ScriptableObject
{
    public string NpcName;

    public Sprite npcSprite;

    public string[] npcDialogues;

    public string[] playerDialogues;

    public float dialogueSpeed;
}