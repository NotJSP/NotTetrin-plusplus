using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StackModeButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("StackModeButton");
    }
}
