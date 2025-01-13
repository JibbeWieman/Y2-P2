using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HelpLineState : StateMachineBehaviour
{
    [SerializeField] protected AudioClip[] sounds;

    public abstract void Play(GameObject gameObject);

    protected int PlaySound(GameObject gameObject)
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        int pickedLine = (int)(Random.value * sounds.Length);
        audio.clip = sounds[pickedLine];
        audio.Play();
        return pickedLine;
    }
}
