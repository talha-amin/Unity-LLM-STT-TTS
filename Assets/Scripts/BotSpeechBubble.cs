using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BotSpeechBubble : MonoBehaviour
{
    [Header("UI References")]
    public Canvas bubbleCanvas;
    public TMP_Text messageText;

    [Header("Settings")]
    public float typingSpeed = 0.03f;
    public int maxVisibleCharacters = 150;  // how many chars visible at once

    Coroutine typingCoroutine;
    string fullMessage = "";

    void Start()
    {
        if (bubbleCanvas) bubbleCanvas.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (bubbleCanvas == null || messageText == null) return;

        bubbleCanvas.gameObject.SetActive(true);
        fullMessage = message;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    public void HideMessage()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (bubbleCanvas != null)
            bubbleCanvas.gameObject.SetActive(false);
    }

    IEnumerator TypeText()
    {
        messageText.text = "";
        var wait = new WaitForSeconds(typingSpeed);

        for (int i = 0; i < fullMessage.Length; i++)
        {
            // If text gets too long, trim from the start
            // so it looks like it's scrolling up
            string current = fullMessage.Substring(0, i + 1);

            if (current.Length > maxVisibleCharacters)
                current = "..." + current.Substring(
                    current.Length - maxVisibleCharacters);

            messageText.text = current;
            yield return wait;
        }
    }
}