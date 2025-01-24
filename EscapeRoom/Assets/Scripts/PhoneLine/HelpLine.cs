using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class HelpLine : MonoBehaviour
{
    [SerializeField] private AudioClip phoneRing;
    private AudioSource audioSource;
    
    private Animator animator;

    private bool isCooldown;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!GetComponent<AudioSource>())
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (phoneRing != null)
        {
            audioSource.clip = phoneRing;
            audioSource.playOnAwake = true;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("PhoneRing audio clip is not assigned!");
        }
    }

    public enum Parameters {
        [Description("loggedIn")]
        LoggedIn,
        [Description("doneWithClues")]
        DoneWithClues,
        [Description("introPlayed")]
        IntroPlayed
    }

    public void Play()
    {
        audioSource.Pause();
        audioSource.clip = null;
        audioSource.loop = false;

        if (isCooldown) return;

        isCooldown = true;
        StartCoroutine(CooldownRoutine());

        var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        

        HelpLineMain helpLineState = animator.GetBehaviours(currentStateInfo.fullPathHash, 0)[0] as HelpLineMain;
        
        if (helpLineState != null)
        {
            helpLineState.Play(gameObject);

            SetParameter(Parameters.IntroPlayed, true);

            if (!helpLineState.AnyValidSounds())
            {
                SetParameter(Parameters.DoneWithClues, true);
            }
        }
        else
        {
            Debug.LogWarning("HelpLineState behavior not found in the animator.");
        }
    }


    public void SetParameter(Parameters parameter, bool value)
    {
        animator.SetBool(ToDescriptionString(parameter), value);
    }

    private static string ToDescriptionString(Parameters val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val
           .GetType()
           .GetField(val.ToString())
           .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(30f);
        isCooldown = false;
    }
}
