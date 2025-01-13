using System.ComponentModel;
using UnityEngine;

public class HelpLine : MonoBehaviour
{
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
        GetComponent<Animator>().GetBehaviour<HelpLineState>().Play(gameObject);
    }

    public void SetParameter(Parameters parameter, bool value)
    {
        GetComponent<Animator>().SetBool(ToDescriptionString(parameter), value);
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
