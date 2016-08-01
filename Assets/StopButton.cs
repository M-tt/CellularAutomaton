using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StopButton : MonoBehaviour
{
    private int previousValue = 0;

    public void mouseClicked()
    {
        Text t = gameObject.GetComponentInChildren<Text>();

        if (GridMasterScript.Instance.updateSkips == int.MaxValue)
        {
            t.text = "Stop";
            GridMasterScript.Instance.updateSkips = previousValue;
        }
        else
        {
            t.text = "Start";
            previousValue = GridMasterScript.Instance.updateSkips;
            GridMasterScript.Instance.updateSkips = int.MaxValue;
        }
    }
}
