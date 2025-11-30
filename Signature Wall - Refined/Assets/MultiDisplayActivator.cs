using UnityEngine;

public class MultiDisplayActivator : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

}
