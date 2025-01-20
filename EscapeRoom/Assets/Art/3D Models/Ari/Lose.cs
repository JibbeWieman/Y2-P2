using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour
{
    [SerializeField] GameObject LoseScreen, text1, text2;

    public void LoseGame() 
    {
        LoseScreen.SetActive(true);
        text1.SetActive(true);
    }
}
