using UnityEngine;

public class SpinnAnimation : MonoBehaviour
{
    private Vector2 startPosition;
    [SerializeField] private float startBounceTime = 0.3f;
    [SerializeField] private float spinTime = 2.0f;
    [SerializeField] private float stopBounceTime = 0.3f;
    [SerializeField] private float spinSpeed = 1000f;
    [SerializeField] private float bounceHeight = 30f;

    private float elapsedTime = 0f;
    private RectTransform rectTransform;
    private enum State { Idle, StartBounce, Spinning, StopBounce, Done }
    private State currentState = State.Idle;
    private float startY;
    private Vector2 targetStopPosition;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        startY = startPosition.y;
        elapsedTime = 0f;
        currentState = State.StartBounce;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        switch (currentState)
        {
            case State.StartBounce:
                {
                    float t = elapsedTime / startBounceTime;
                    if (t >= 1f)
                    {
                        t = 1f;
                        elapsedTime = 0f;
                        currentState = State.Spinning;
                    }
                    float bounce = Mathf.Sin(t * Mathf.PI) * bounceHeight;
                    rectTransform.anchoredPosition = new Vector2(startPosition.x, startY + bounce);
                    break;
                }

            case State.Spinning:
                {
                    float t = Mathf.Clamp01(elapsedTime / spinTime);
                    float speed = Mathf.Lerp(0, spinSpeed, t);
                    rectTransform.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

                    if (elapsedTime >= spinTime)
                    {
                        elapsedTime = 0f;
                        currentState = State.StopBounce;

                        // Calculate snapped stop position
                        float currentY = rectTransform.anchoredPosition.y;
                        float snappedY = Mathf.Round(currentY / 170f) * 170f;
                        targetStopPosition = new Vector2(startPosition.x, snappedY);
                    }
                    break;
                }

            case State.StopBounce:
                {
                    float t = elapsedTime / stopBounceTime;
                    if (t >= 1f)
                    {
                        t = 1f;
                        currentState = State.Done;
                        rectTransform.anchoredPosition = targetStopPosition;
                        return;
                    }
                    float bounce = Mathf.Sin(t * Mathf.PI) * bounceHeight;
                    rectTransform.anchoredPosition = new Vector2(targetStopPosition.x, targetStopPosition.y + bounce);
                    break;
                }

            case State.Done:
                this.enabled = false;
                break;
        }
    }
}
