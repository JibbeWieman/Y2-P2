//using TMPro;
//using UnityEngine;

//public class UI_InputField : MonoBehaviour
//{
//    [SerializeField]
//    private TMP_InputField inputField;

//    [SerializeField]
//    private int characterLimit;

//    [SerializeField]
//    private string targetString = "MatchMe"; // The string to match against.

//    private bool isMatch = false; // Bool to track the match status.

//    void Start()
//    {
//        inputField.characterLimit = characterLimit;
//        inputField.onSubmit.AddListener(ValidateInput);
//    }

//    private void ValidateInput(string inputText)
//    {
//        if (inputText == targetString)
//        {
//            isMatch = true;
//            Debug.Log("Text matches the target string!");
//        }
//        else
//        {
//            isMatch = false;
//            Debug.Log("Text does not match the target string.");
//        }
//    }
//}
