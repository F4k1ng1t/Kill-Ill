using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI NPC_Name;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void NPCSpeak(string NPC_Name, string dialogue)
    {
        this.NPC_Name.SetText(NPC_Name);
        this.dialogue.SetText(dialogue);
    }
}
