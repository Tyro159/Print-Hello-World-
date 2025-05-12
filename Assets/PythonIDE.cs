using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Python.Runtime;  // Import Python.NET
using System;
using System.Collections.Generic;
using System.IO;

public class PythonIDE : MonoBehaviour
{
    public TMP_InputField codeInputField;
    public TMP_InputField outputText;
    public TMP_Text completionMark;

    // Dictionary to store expected outputs for each challenge
    private Dictionary<string, string> challengeExpectedOutputs = new Dictionary<string, string>
    {
        { "Challenge1", "Hello, World!\n" },
        { "Challenge2", "6 2 8 2.0\n" },
        { "Challenge3", "1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n" },
        { "Challenge4", "['Apple', 'Orange', 'Banana', 'Peach', 'Pear']\n" },
        { "Challenge5", "Does 1 minus 4 equal 0? False\n" },
        { "Challenge6", "Alice Smith is 25 years old.\n" },
        { "Challenge7", "This Is A Dictionary" },
        { "Challenge8", $"Today's date is: {DateTime.Today:MM-dd-yyyy}\n" },
        { "Challenge9", $"Today's date is: {DateTime.Today:MM-dd-yyyy}\nNext month: {DateTime.Today.AddMonths(1):MM-dd-yyyy}\n" },
        { "Challenge10", "Found the correct number!\n" },
        { "Challenge11", "    *\n   **\n  ***\n ****\n*****\n" },
        {"Challenge12",
            "1\n2\nFizz\n4\nBuzz\nFizz\n7\n8\nFizz\nBuzz\n11\nFizz\n13\n14\nFizzBuzz\n" +
            "16\n17\nFizz\n19\nBuzz\nFizz\n22\n23\nFizz\nBuzz\n26\nFizz\n28\n29\nFizzBuzz\n" +
            "31\n32\nFizz\n34\nBuzz\nFizz\n37\n38\nFizz\nBuzz\n41\nFizz\n43\n44\nFizzBuzz\n" +
            "46\n47\nFizz\n49\nBuzz\n"
        },
        { "Challenge13", "2\n3\n5\n7\n11\n13\n17\n19\n23\n29\n31\n37\n41\n43\n47" },
        { "Challenge14", "Original list: [1, 2, 3, 4, 5]\nReversed list: [5, 4, 3, 2, 1]\n" },
        { "Challenge15", "Factorial of 5 is 120\n" },
        { "Challenge16", "Original list: [7, 2, 9, 1, 5]\nSorted list: [1, 2, 5, 7, 9]\n" },
        { "Challenge17", "0 1 1 2 3 5 8 13 21 34" },
        { "Challenge18", "Dictionary 1: {'a': 1, 'b': 2, 'c': 3}\nDictionary 2: {'d': 4, 'e': 5, 'f': 6}\nMerged Dictionary: {'a': 1, 'b': 2, 'c': 3, 'd': 4, 'e': 5, 'f': 6}" },
        { "Challenge19", "Original Matrix:\n1 2 3\n4 5 6\n7 8 9\nTransposed Matrix:\n1 4 7\n2 5 8\n3 6 9\n" },
        { "Challenge20", "Original Message: Fdhvdu Flskhu\nDecrypted Message: Caesar Cipher"}
    };

    private string currentChallenge;

    void Start()
    {

        // Initialize Python.NET
        string pythonDllPath;

        #if UNITY_EDITOR
            pythonDllPath = Path.Combine(Application.dataPath, "PythonEmbed", "python39.dll");
        #else
            pythonDllPath = Path.Combine(Application.dataPath, "..", "PythonEmbed", "python39.dll");
        #endif

        Python.Runtime.Runtime.PythonDLL = pythonDllPath;
        PythonEngine.Initialize();

        using (Py.GIL())
        {
            dynamic sys = Py.Import("sys");

            Debug.Log("Python version: " + sys.version);
            Debug.Log("Python executable: " + sys.executable);
            Debug.Log("Python path: " + sys.path);
        }

        codeInputField = GameObject.Find("CodeInputField").GetComponent<TMP_InputField>();
        outputText = GameObject.Find("OutputText").GetComponent<TMP_InputField>();
        completionMark = GameObject.Find("completionMark").GetComponent<TMP_Text>();

        // Add debug logs to check if the elements are assigned
        if (codeInputField == null) Debug.LogError("CodeInputField not found.");
        if (outputText == null) Debug.LogError("OutputText not found.");
        if (completionMark == null) Debug.LogError("completionMark not found.");

        completionMark.gameObject.SetActive(false);  // Hide completion mark initially
    }

    // Function to execute Python code
    public void RunCode()
    {
        // Set the current challenge based on the active scene name
        SetCurrentChallenge(SceneManager.GetActiveScene().name);
        Debug.Log("Current Challenge: " + currentChallenge);

        if (codeInputField == null || outputText == null || completionMark == null)
        {
            Debug.LogError("UI elements not found.");
            return;
        }

        string code = codeInputField.text;
        Debug.Log("Running code: " + code);

        using (Py.GIL())
        {
            try
            {
                // Use Python to redirect standard output and execute user code
                string fullScript = $@"
import sys
from io import StringIO

old_stdout = sys.stdout
sys.stdout = mystdout = StringIO()

try:
    exec('''{code}''')
    output = mystdout.getvalue()
finally:
    sys.stdout = old_stdout
";

                // Execute script and capture output
                dynamic scope = Py.CreateScope();
                scope.Exec(fullScript);

                // Put output into console view
                dynamic output = scope.Get("output");
                string outputStr = output.ToString();
                outputText.text = outputStr;

                Debug.Log("Python result: " + outputStr);

                // Get expected output for current challenge
                string expectedOutput = GetExpectedOutputForCurrentChallenge();

                // Compare output with expected output
                if (outputStr == expectedOutput)
                {
                    completionMark.gameObject.SetActive(true);
                    UnlockChallengeAchievement(); // Unlock associated achievement with challenge 
                }

                else if (outputStr.Contains(expectedOutput))
                   {
                    completionMark.gameObject.SetActive(true);
                    UnlockChallengeAchievement(); // Unlock associated achievement with challenge 
                }

                else
                {
                    completionMark.gameObject.SetActive(false);
                }
            }
            catch (PythonException e)
            {
                // Display Python errors in Output Text field
                outputText.text = $"Error: {e.Message}";
                Debug.LogError("Python error: " + e.Message);
                completionMark.gameObject.SetActive(false);
            }
        }
    }

    void OnApplicationQuit()
    {
        // Shutdown Python engine when app closes
        PythonEngine.Shutdown();
    }

    // Set current challenge based on scene name
    private void SetCurrentChallenge(string sceneName)
    {
        Debug.Log("Active Scene Name: " + sceneName);

        if (challengeExpectedOutputs.ContainsKey(sceneName) || sceneName == "Challenge4" || sceneName == "Challenge5")
        {
            currentChallenge = sceneName;
            Debug.Log("Challenge Set: " + currentChallenge);
        }
        else
        {
            Debug.LogError("Challenge not found for scene: " + sceneName);
            currentChallenge = null;
        }
    }

    // Get expected output for current challenge
    private string GetExpectedOutputForCurrentChallenge()
    {
        if (challengeExpectedOutputs.TryGetValue(currentChallenge, out string expectedOutput))
        {
            return expectedOutput;
        }

        return null;  // Return null if no expected output is found
    }


    private void UnlockChallengeAchievement()
    {
        string achievementName = currentChallenge switch
        {
            "Challenge1" => "Unlocked the Terminal",
            "Challenge2" => "The Calculator",
            "Challenge3" => "Looping, Looping, Looping",
            "Challenge4" => "Making a list",
            "Challenge5" => "Logic Thinker",
            "Challenge6" => "Putting words in your Mouth",
            "Challenge7" => "Capital Planner",
            "Challenge8" => "Date Detective",
            "Challenge9" => "Future Predictor",
            "Challenge10" => "Randomness at its finest",
            "Challenge11" => "Pyramid Builder",
            "Challenge12" => "A Common Demoninator",
            "Challenge13" => "Prime Finder",
            "Challenge14" => "Reversal",
            "Challenge15" => "Infinite Recursion",
            "Challenge16" => "Everything can be Sorted",
            "Challenge17" => "The Fibonacci Sequence",
            "Challenge18" => "Dictionary Detective",
            "Challenge19" => "Into the Matrix",
            "Challenge20" => "Decipher like Caesar",
            _ => null
        };

        if (achievementName != null)
        {
            AchievementManager.Instance.UnlockAchievement(achievementName);
        }
    }
}
