using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField] protected NpcSO npc;

    #region Dialogue System

    [SerializeField] protected GameObject panel;

    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected Image speakerImage;

    protected int currentDialogueIndex;
    protected bool isChatting;
    [SerializeField] protected string currentDialogue;
    protected string currentSpeaker;

    [SerializeField] protected Sprite playerSprite;
    #endregion

    public Animator animator;


    private void Start()
    {
        playerSprite = GameObject.FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sprite;
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        DialogueManage();
    }


    protected virtual IEnumerator Dialogue()
    {
        text.SetText("");
        if (currentDialogue.Equals(string.Empty))
        {
            speakerImage.sprite = npc.npcSprite;
            currentSpeaker = npc.NpcName + " : ";
            currentDialogue = npc.npcDialogues[0];
        }

        isChatting = true;

        char[] tempDialogue = (currentSpeaker + currentDialogue).ToCharArray();

        foreach (char c in tempDialogue)
        {
            text.SetText(text.text + c);
            yield return new WaitForSeconds(npc.dialogueSpeed);
        }

        if (currentDialogueIndex == npc.npcDialogues.Length) isChatting = false;
    }

    protected void DialogueManage()
    {
        if (!Input.GetMouseButtonDown(0) | !isChatting) return;

        if (currentDialogue.Equals(npc.playerDialogues[^1].ToString())) LeaveDialogue(panel);

        if (isChatting)
        {
            if (currentDialogue.Equals(npc.npcDialogues[currentDialogueIndex]))
            {
                currentSpeaker = "Player : "; currentDialogue = npc.playerDialogues[currentDialogueIndex];
                speakerImage.sprite = playerSprite;
            }
            else
            {
                currentDialogueIndex++;
                speakerImage.sprite = npc.npcSprite;
                currentDialogueIndex = Mathf.Clamp(currentDialogueIndex, 0, npc.npcDialogues.Length - 1);
                currentSpeaker = npc.NpcName + " : ";
                currentDialogue = npc.npcDialogues[currentDialogueIndex];
            }
            text.SetText(currentDialogue);

            StopAllCoroutines();
            StartCoroutine(nameof(Dialogue));
        }
    }
    protected virtual void LeaveDialogue(GameObject dialoguePanel)
    {
        dialoguePanel.SetActive(false);
        text.SetText(string.Empty);
        currentDialogue = string.Empty;
        currentDialogueIndex = 0;
        isChatting = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collisionIsPlayer(collision)) return;
        panel.SetActive(true);
        StartCoroutine(nameof(Dialogue));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collisionIsPlayer(collision)) return;
        panel.SetActive(false);
        isChatting = false;
        StopAllCoroutines();
    }

    private bool collisionIsPlayer(Collider2D collision) => collision.GetComponent<Player>() != null;
}