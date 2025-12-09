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

    // Button click functions with fade
    public void CharacterOne() { StartCoroutine(LoadSceneWithFade(1)); }
    public void CharacterTwo() { StartCoroutine(LoadSceneWithFade(2)); }
    public void CharacterThree() { StartCoroutine(LoadSceneWithFade(3)); }

    private IEnumerator LoadSceneWithFade(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);

        if (fadeImage != null)
        {
            // Fade to black
            LeanTween.alpha(fadeImage.rectTransform, 1f, fadeDuration).setEase(LeanTweenType.easeInOutQuad);
            yield return new WaitForSeconds(fadeDuration);
        }

        // Load scene after fade
        SceneManager.LoadScene("Map2");
    }
}
