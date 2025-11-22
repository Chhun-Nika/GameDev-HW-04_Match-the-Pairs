using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    public int cardID;               // Set in Inspector: same ID means match
    public Sprite questionSprite;    // Card back sprite (e.g. question mark)
    public Sprite faceSprite;        // Card front sprite (e.g. flower)
    public bool isMatched = false;

    private Image img;
    private bool isFlipped = false;

    private GameManager gameManager;

    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = questionSprite;

        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnClick()
    {
        if (isFlipped || isMatched || !gameManager.CanSelect())
            return;

        StartCoroutine(FlipAnimation());
        isFlipped = true;

        gameManager.CardSelected(this);
    }

    IEnumerator FlipAnimation()
    {
        // Shrink width to 0
        for (float scale = 1f; scale >= 0f; scale -= Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        img.sprite = faceSprite;

        // Grow width back to 1
        for (float scale = 0f; scale <= 1f; scale += Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    public void FlipBack()
    {
        StartCoroutine(FlipBackAnimation());
        isFlipped = false;
    }

    IEnumerator FlipBackAnimation()
    {
        // Shrink width to 0
        for (float scale = 1f; scale >= 0f; scale -= Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        img.sprite = questionSprite;

        // Grow width back to 1
        for (float scale = 0f; scale <= 1f; scale += Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    public IEnumerator DisappearAnimation()
    {
        Image img = GetComponent<Image>();
        Color originalColor = img.color;
        float duration = 0.8f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            float scale = Mathf.Lerp(1f, 0f, elapsed / duration);
            transform.localScale = new Vector3(scale, scale, 1);

            yield return null;
        }

        gameObject.SetActive(true);
    }

}
