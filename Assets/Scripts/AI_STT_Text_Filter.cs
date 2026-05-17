using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_STT_Text_Filter : MonoBehaviour
{
    public enum PreFilterClass { isSpeech };      //add more when needed

    [SerializeField] bool debug;
    const string DEBUG_PREFIX = "AI_STT_Text_Filter: ";

    AI_Orchestrator aiO;
ChatUI chatUI;    // ← add this line


    private void Start()
{
    aiO = GetComponent<AI_Orchestrator>();
    if (!aiO)
    {
        Debug.LogError("AI Orchestrator component not found!");
        return;
    }

    chatUI = FindObjectOfType<ChatUI>();    // ← add this line
}


    public async void DirectToCloudProviders(string sttResponseText)
{
    string context = "";

    if (debug)
        Debug.Log(DEBUG_PREFIX + "IS_SPEECH");

    // Show user message in chat UI
    if (chatUI) chatUI.AddUserMessage(sttResponseText);

    if (aiO.RAGConfigured())
        context = await aiO.RAGGetContext(sttResponseText, aiO.maxResults);

    if (debug)
        Debug.Log(DEBUG_PREFIX + "Prompt:" + sttResponseText + ", Context:" + context);

    aiO.TextToLLM(sttResponseText, context);
}
}
