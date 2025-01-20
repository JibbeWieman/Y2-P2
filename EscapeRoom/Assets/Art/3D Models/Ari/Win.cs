using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    public void ShowWin() 
    {
        WinScreen.SetActive(true);
    }
}
