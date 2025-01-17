using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HelpLineState : StateMachineBehaviour
{
    [SerializeField] protected AudioClip[] sounds;

    [SerializeField] protected bool removeSoundOnPlay;

    public abstract int Play(GameObject gameObject);

    protected int SelectRandomAudioId()
    {
        int pickedLine = (int)(Random.value * sounds.Length);
        if (sounds[pickedLine] == null)
        {
            return SelectRandomAudioId();
        }

        return pickedLine;
    }

    protected int PlaySound(GameObject gameObject)
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        int pickedLine = SelectRandomAudioId();
        audio.clip = sounds[pickedLine];
        if (removeSoundOnPlay) sounds[pickedLine] = null;
        audio.Play();
        return pickedLine;
    }

    public bool AnyValidSounds()
    {
        if (sounds == null || sounds.Length == 0)
        {
            return false; // No sounds available.
        }

        foreach (AudioClip clip in sounds)
        {
            if (clip != null)
            {
                return true; // At least one valid sound exists.
            }
        }

        return false; // No non-null sounds found.
    }
}
