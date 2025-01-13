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

    public void play()
    {
        GetComponent<Animator>().GetBehaviour<HelpLineState>().play(gameObject);
    }

    public void setParameter(Parameters parameter, bool value)
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
