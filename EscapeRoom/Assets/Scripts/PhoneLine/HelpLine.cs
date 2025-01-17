using System.ComponentModel;
using UnityEngine;

public class HelpLine : MonoBehaviour
{
    private Animator animator;

    //public bool checkLastEdited = false;
    //public bool unlockedDrawer = false;
    //public bool foundQRCode = false;


    public void Start()
    {
        animator = GetComponent<Animator>();
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
        animator.GetBehaviour<HelpLineState>().Play(gameObject);
        SetParameter(Parameters.IntroPlayed, true);
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
}
