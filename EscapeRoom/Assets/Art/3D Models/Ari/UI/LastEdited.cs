using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEdited : MonoBehaviour
{
    [SerializeField] public GameObject JeffPanel, AnnaPanel, RenePanel, ThomasPanel;
    private Randomizer randomizer;

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
