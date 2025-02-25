using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpLineMain : HelpLineState
{
    override public int Play(GameObject gameObject)
    {
        return PlaySound(gameObject);
    }
}
