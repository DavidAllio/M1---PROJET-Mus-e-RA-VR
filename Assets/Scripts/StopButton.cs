using UnityEngine.UI;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    public GiveINFO givI;
    public void closeMedias()
    {
        givI.StopAll();
    }
}
