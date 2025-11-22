using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public CardFlip firstCard;
    public CardFlip secondCard;
    public TMP_Text scoreText;
    public RectTransform scoreTextRect;      // Assign TMP Text RectTransform here
    public CardFlip[] allCards;               // Assign all your cards here

    public Vector3 centerPosition = Vector3.zero;  // Center position for score text
    public float moveDuration = 1f;

    private int score = 0;
    private bool canSelect = true;

    public GameObject buttonScene1;
    public GameObject buttonScene2;


    public bool CanSelect()
    {
        return canSelect;
    }

    public void CardSelected(CardFlip card)
    {
        if (firstCard == null)
        {
            firstCard = card;
            return;
        }

        if (secondCard == null && card != firstCard)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        canSelect = false;

        yield return new WaitForSeconds(1f);

        if (firstCard.cardID == secondCard.cardID)
        {
            firstCard.isMatched = true;
            secondCard.isMatched = true;

            score += 10;
            UpdateScore();

            StartCoroutine(firstCard.DisappearAnimation());
            StartCoroutine(secondCard.DisappearAnimation());

            yield return new WaitForSeconds(0.5f);

            firstCard = null;
            secondCard = null;

            canSelect = true;

            if (AreAllCardsMatched())
            {
                StartCoroutine(MoveScoreTextToCenter());
            }
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();

            firstCard = null;
            secondCard = null;

            canSelect = true;
        }
    }

    bool AreAllCardsMatched()
    {
        foreach (CardFlip card in allCards)
        {
            if (!card.isMatched)
                return false;
        }
        return true;
    }

    IEnumerator MoveScoreTextToCenter()
    {
        Vector3 startPos = scoreTextRect.localPosition;  // current position
        Vector3 endPos = new Vector3(0, 160f);                    // center of canvas (adjust if needed)
        float duration = 1f;                              // animation duration
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scoreTextRect.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }
        scoreTextRect.localPosition = endPos;

        buttonScene1.SetActive(true);
        buttonScene2.SetActive(true);
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score " + score;
    }
}
