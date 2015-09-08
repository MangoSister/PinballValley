using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour
{
    public void OnClickStart()
    {
        Application.LoadLevel("regular");
    }


    public void OnClickExit()
    {
        Application.Quit();
    }
}
