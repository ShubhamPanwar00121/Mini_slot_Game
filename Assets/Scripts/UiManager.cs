using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text betText, creditText;

    public void Spinn()
    {
        CustomEvents.InvokeSpinn();
    }
}
