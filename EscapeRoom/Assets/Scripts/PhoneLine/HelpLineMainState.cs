using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpLineMain : HelpLineState
{
    override public void Play(GameObject gameObject)
    {
        PlaySound(gameObject);
    }
}
