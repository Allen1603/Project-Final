using UnityEngine;

public class CharacterPick : MonoBehaviour
{
    public GameObject Character1;
    public GameObject Character2;
    public GameObject Character3;

    void Start()
    {
        DisableAll();

        int selected = PlayerPrefs.GetInt("SelectedCharacter", 1);

        switch (selected)
        {
            case 1:
                Character1.SetActive(true);
                break;

            case 2:
                Character2.SetActive(true);
                break;

            case 3:
                Character3.SetActive(true);
                break;
        }
    }

    void DisableAll()
    {
        Character1.SetActive(false);
        Character2.SetActive(false);
        Character3.SetActive(false);
    }
}