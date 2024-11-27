using UnityEngine;

public class IndicatorControl : MonoBehaviour
{
    Transform indicator; // The UI element for the indicator
    public float shrinkRate = 7f; // Rate of shrinking per second
    public float shrinkMinSize = 0.1f; // Minimum size before spell fizzles
    public float growDuration = 0.2f; // Duration of the growth animation

    private Vector3 originalScale; // Original size of the indicator

    void Start()
    {
        ResetSize();
        indicator = transform;
        originalScale = indicator.localScale;
        StartShrinking();
    }

    void StartShrinking()
    {
        LeanTween.scale(indicator.gameObject, Vector3.one * shrinkMinSize, shrinkRate)
            .setEaseLinear()
            .setOnComplete(SpellFizzled);
    }

    public void UpdateSize(float scale)
    {
        // Grow the indicator when the player inputs a correct word
        Vector3 targetScale = new Vector3(
            Mathf.Min(indicator.localScale.x + scale, originalScale.x),
            Mathf.Min(indicator.localScale.y + scale, originalScale.y),
            Mathf.Min(indicator.localScale.z + scale, originalScale.z)
        );

        LeanTween.cancel(indicator.gameObject); // Cancel any ongoing tweens on the indicator
        LeanTween.scale(indicator.gameObject, targetScale, growDuration).setEaseOutCubic().setOnComplete(StartShrinking); // Resume shrinking after growth;
    }

    public void ResetSize()
    {
        if (this != null)
            transform.localScale = new Vector3(1, 1, 1);
    }
    private void SpellFizzled()
    {
        Debug.Log("The spell fizzled out!");
        LeanTween.cancel(indicator.gameObject); // Stop all ongoing tweens
        //Destroy(gameObject);
        // Add any additional logic for spell failure (e.g., resetting or ending the spell)
    }
}
