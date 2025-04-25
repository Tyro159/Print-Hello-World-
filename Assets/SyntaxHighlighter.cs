using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class SyntaxHighlighter : MonoBehaviour
{
    public TMP_InputField codeInputField;
    public TMP_Text highlightedDisplay;

    void Update()
    {
        // Check for Enter key press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HighlightSyntax(codeInputField.text);
        }
    }

    void HighlightSyntax(string text)
    {
        highlightedDisplay.text = ApplySyntaxHighlighting(text);
    }

    string ApplySyntaxHighlighting(string text)
    {
        // Define colors (modify these as you like)
        string methodColor = "<color=#569CD6>";  // Blue
        string commentColor = "<color=#919191>"; // Gray
        string stringColor = "<color=#6A9955>";  // Green
        string resetColor = "</color>";

        var methodPattern = new Regex(@"\b\w+(?=\()");
        var commentPattern = new Regex(@"#.*");
        var stringPattern = new Regex(@"(['""]).*?\1");

        text = methodPattern.Replace(text, m => $"{methodColor}{m.Value}{resetColor}");
        text = commentPattern.Replace(text, m => $"{commentColor}{m.Value}{resetColor}");
        text = stringPattern.Replace(text, m => $"{stringColor}{m.Value}{resetColor}");

        return text;
    }
}
