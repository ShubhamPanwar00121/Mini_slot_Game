using System.Collections;
using UnityEngine;

public class SpinnManager : MonoBehaviour
{
    private WaitForSeconds inBetweenDelay = new WaitForSeconds(0.5f);

    private void OnEnable()
    {
        CustomEvents.spinn += PerformSpinn;
    }

    private void OnDisable()
    {
        CustomEvents.spinn -= PerformSpinn;
    }

    private void PerformSpinn()
    {
        StartCoroutine(SpinnCoroutine());
    }

    private IEnumerator SpinnCoroutine()
    {
        foreach (Transform column in transform)
        {
            if (column.TryGetComponent<SpinnAnimation>(out SpinnAnimation sa))
            {
                sa.enabled = true;
                yield return inBetweenDelay;
            }
        }
        yield return null;
    }
}
