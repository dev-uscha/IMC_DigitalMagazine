using UnityEngine;

public class ScaleGrow : MonoBehaviour
{
    public Transform startSize;
    public Transform endSize;
    public float growDuration = 1.0f;
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


    private Vector3 fromScale;
    private Vector3 toScale;
    private float timeElapsed;
    private bool isAnimating = false;

    void Update()
    {
        if (isAnimating)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / growDuration);
            float curvedT = animationCurve.Evaluate(t);
            transform.localScale = Vector3.Lerp(fromScale, toScale, curvedT);

            if (t >= 1.0f)
            {
                isAnimating = false;
            }
        }
    }

    public void MoveToEnd()
    {
        StartScaling(transform.localScale, endSize.localScale);
    }

    public void MoveToStart()
    {
        StartScaling(transform.localScale, startSize.localScale);
    }

    private void StartScaling(Vector3 from, Vector3 to)
    {
        fromScale = from;
        toScale = to;
        timeElapsed = 0f;
        isAnimating = true;
    }
}
