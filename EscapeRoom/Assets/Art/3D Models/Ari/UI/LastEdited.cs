using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEdited : MonoBehaviour
{
    [SerializeField] public GameObject buttonOne, buttonTwo, JeffPanel, AnnaPanel, RenePanel, ThomasPanel;
    private Randomizer randomizer;

    private void Update()
    {
        if (JeffPanel.activeSelf || AnnaPanel.activeSelf || RenePanel.activeSelf || ThomasPanel.activeSelf) 
        {
            buttonOne.SetActive(false);
            buttonTwo.SetActive(false);
        }
        else 
        {
            buttonOne.SetActive(true);
            buttonTwo.SetActive(true);
        }
    }
    public void LastEditedPanel()
    {
        randomizer = FindAnyObjectByType<Randomizer>();

        if (randomizer.hackerName == "Jeff Jansen")
        {
            JeffPanel.SetActive(true);
        }
        else if (randomizer.hackerName == "Anna Smid")
        {
            AnnaPanel.SetActive(true);
        }
        else if (randomizer.hackerName == "René Rijkerboer") 
        {
            RenePanel.SetActive(true);
        }
        else ThomasPanel.SetActive(true);
    }

}
