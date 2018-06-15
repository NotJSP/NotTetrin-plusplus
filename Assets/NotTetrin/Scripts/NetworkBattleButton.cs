using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkBattleButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("NetworkBattleButton");
    }
}
