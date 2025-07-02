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
        SpinnAnimation lastAnim = null;

        foreach (Transform column in transform)
        {
            if (column.TryGetComponent<SpinnAnimation>(out SpinnAnimation sa))
            {
                sa.enabled = true;
                lastAnim = sa;
                yield return inBetweenDelay;
            }
        }

        yield return null;

        if (lastAnim != null)
        {
            yield return new WaitUntil(() => lastAnim.AnimationComplete);
        }

        CardBehaviour[,] finalMatrix = GetFinalMatrix();
        CustomEvents.InvokeSpinnComplete(finalMatrix);
    }

    private CardBehaviour[,] GetFinalMatrix()
    {
        CardBehaviour[,] cardMatrix;
        SpinnAnimation[] columns = GetComponentsInChildren<SpinnAnimation>();

        if (columns.Length == 0)
        {
            return null;
        }

        int columnCount = columns.Length;
        int rowCount = columns[0].GetCards().Count;

        cardMatrix = new CardBehaviour[columnCount, rowCount];

        for (int x = 0; x < columnCount; x++)
        {
            SpinnAnimation column = columns[x];

            for (int y = 0; y < rowCount; y++)
            {
                cardMatrix[x, y] = column.GetCards()[y];
            }
        }

        #region debug
        //for (int y = 0; y < cardMatrix.GetLength(1); y++) 
        //{
        //    string rowOutput = "";

        //    for (int x = 0; x < cardMatrix.GetLength(0); x++)
        //    {
        //        CardBehaviour card = cardMatrix[x, y];
        //        rowOutput += card != null ? card.Index + "\t" : "null\t";
        //    }

        //    Debug.Log($"Row {y}: {rowOutput}");
        //}
        #endregion

        return cardMatrix;
    }
}
