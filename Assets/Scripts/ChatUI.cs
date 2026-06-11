// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class ChatUI : MonoBehaviour
// {
//     public Transform content;

//     public GameObject userMessagePrefab;
//     public GameObject botMessagePrefab;

//     public TMP_InputField inputField;

//     public ScrollRect scrollRect;

//     // USER MESSAGE
//     public void AddUserMessage(string message)
//     {
//         GameObject msg = Instantiate(userMessagePrefab, content);

//         TMP_Text text = msg.GetComponentInChildren<TMP_Text>();
//         text.text = message;

//         ScrollToBottom();
//     }

//     // BOT MESSAGE
//     public void AddBotMessage(string message)
//     {
//         GameObject msg = Instantiate(botMessagePrefab, content);

//         TMP_Text text = msg.GetComponentInChildren<TMP_Text>();
//         text.text = message;

//         ScrollToBottom();
//     }

//     void ScrollToBottom()
//     {
//         Canvas.ForceUpdateCanvases();
//         scrollRect.verticalNormalizedPosition = 0f;
//     }

//     // CALLED WHEN ENTER IS PRESSED
//     public void SendMessage()
// {
//     string message = inputField.text;

//     if (string.IsNullOrWhiteSpace(message))
//         return;

//     AddUserMessage(message);

//     inputField.text = "";

//     inputField.ActivateInputField();

//     AddBotMessage("AI reply...");
// }
// }

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUI : MonoBehaviour
{
    public Transform content;

    public GameObject userMessagePrefab;

    public GameObject botMessagePrefab;

    public ScrollRect scrollRect;

    // USER MESSAGE
    public void AddUserMessage(string message)
    {
        AddMessage(userMessagePrefab, message);
    }

    // BOT MESSAGE
    public void AddBotMessage(string message)
    {
        AddMessage(botMessagePrefab, message);
    }

    // INTERNAL
    void AddMessage(GameObject prefab, string message)
    {
        GameObject newMsg =
            Instantiate(prefab, content);

        TMP_Text text =
            newMsg.GetComponentInChildren<TMP_Text>();

        text.text = message;

        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 0f;
    }
}