using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwipePage : MonoBehaviour
{
    public static SwipePage instance;

    public int maxPage;
    private int currentPage;
    private Vector3 targetPos;
    public Vector3 pageStep;
    public RectTransform levelPageRect;
    public float tweenTime;
    public LeanTweenType tweenType;
    public Camera cameraMovement;

    [Header("Fade Transition")]
    public Image fadeImage; // Assign a full screen black UI Image
    public float fadeDuration = 1f; // How long the fade lasts

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPageRect.localPosition;
    }

    private void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBGM("PickingAudio");

        // Make sure fade starts transparent
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
            MoveCamera();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
            MoveCamera();
        }
    }

    void MovePage()
    {
        levelPageRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    void MoveCamera()
    {
        Vector3 camPos = cameraMovement.transform.position;

        if (currentPage == 3)
            camPos.x = 8.3f;
        else if (currentPage == 2)
            camPos.x = 2.7f;
        else if (currentPage == 1)
            camPos.x = -3.07f;

        LeanTween.move(cameraMovement.gameObject, camPos, tweenTime).setEase(tweenType);
    }

    // ---------- CHARACTER PICK ----------
    public void CharacterOne() => PickCharacter(1);
    public void CharacterTwo() => PickCharacter(2);
    public void CharacterThree() => PickCharacter(3);

    void PickCharacter(int index)
    {
        PlayerPrefs.SetInt("SelectedCharacter", index);
        StartCoroutine(LoadSelectedPond());
    }

    IEnumerator LoadSelectedPond()
    {
        if (fadeImage != null)
        {
            LeanTween.alpha(fadeImage.rectTransform, 1f, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        int pond = PlayerPrefs.GetInt("SelectedPond", 1);

        SceneManager.LoadScene(pond == 1 ? "Map1" : "Map2");
    }
}
