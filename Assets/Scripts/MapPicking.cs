using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPicking : MonoBehaviour
{
    public void SelectPond(int pondIndex)
    {
        // Save selected pond
        PlayerPrefs.SetInt("SelectedPond", pondIndex);

        // Always go to character picking after pond selection
        SceneManager.LoadScene("PickingCharacter");
    }

    public void Pond1() => SelectPond(1);
    public void Pond2() => SelectPond(2);
}
