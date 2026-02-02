using TMPro;
using UnityEngine;

public class NPCPersona : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private TextMeshProUGUI nameText;
    public string NPC_name;
    public string dialogue;
    public bool infected = false;

    [Header("Gender")]
    [SerializeField] private GameObject maleModel;
    [SerializeField] private GameObject femaleModel;
    private GameObject activeGenderModel;
    private enum Gender
    {
        Male,
        Female
    }
    [SerializeField] private Gender gender = Gender.Male;


    [Header("Textures")]
    [SerializeField] private Material[] maleTextures;
    [SerializeField] private Material[] femaleTextures;


    private void ChooseTexture(Material[] textures)
    {
        int newTexture = Random.Range(0, textures.Length);

        for (int i = activeGenderModel.transform.childCount - 1; i >= 0; i--)
        {
            Renderer renderer = activeGenderModel.transform.GetChild(i).GetComponent<Renderer>();
            renderer.material = textures[newTexture];
        }
    }

    private void Awake()
    {
        //Apply gender model and random texture
        switch (gender)
        {
            case Gender.Male:
                activeGenderModel = maleModel;
                femaleModel.SetActive(false);
                ChooseTexture(maleTextures);
                break;

            case Gender.Female:
                activeGenderModel = femaleModel;
                maleModel.SetActive(false);
                ChooseTexture(femaleTextures);
                break;
        }

        nameText.SetText(NPC_name);
    }

    public void Interact()
    {
        UIManager.Instance.NPCSpeak(NPC_name, dialogue);
        GetComponent<NPCPathfinding>().StartInteraction();
    }

    public void Leave()
    {
        GetComponent<NPCPathfinding>().EndInteraction();
    }
}
