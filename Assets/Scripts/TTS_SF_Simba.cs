using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class TTS_SF_Simba : MonoBehaviour
{
    private string SPEECHIFY_API_KEY;

    private enum SelectVoice
    {
        NL_Daan, NL_Lotte, US_Henry, US_Carly, US_Kyle, US_Kristy, US_Oliver, US_Tasha, US_Joe, US_Lisa,
        US_George, US_Emily, US_Rob, GB_Russell, GB_Benjamin, GB_Michael, AU_KIM, IN_Ankit, IN_Arun,
        GB_Carol, GB_Helen, US_Julie, AU_Linda, US_Mark, US_Nick, NG_Elijah, GB_Beverly, GB_Collin,
        US_Erin, US_Jack, US_Jesse, US_Keenan, US_Lindsey, US_Monica, GB_Phil, GB_Declan, US_Stacy,
        GB_Archie, US_Evelyn, GB_Freddy, GB_Harper, US_Jacob, US_James, US_Mason, US_Victoria
    }

    private enum SelectModel
    {
        _base, _english, _multilingual, _turbo
    }

    [SerializeField] private SelectVoice selectVoice;
    [SerializeField] private SelectModel selectModel;

    const string TTS_API_URI = "https://api.sws.speechify.com/v1/audio/speech";
    private string sfVoice;
    private string sfModel;
    [SerializeField] private Animator avtAnimator;
    API_Keys api_Keys;

    [SerializeField] private bool debug;
    const string DEBUG_PREFIX = "TTS_SF_SIMBA: ";


    public void Init()
    {
        api_Keys = GetComponent<API_Keys>();
        if (!api_Keys)
            Debug.LogError(DEBUG_PREFIX + "Cannot find the API Keys component!");
        else SPEECHIFY_API_KEY = api_Keys.GetAPIKey("Speechify_API_Key");

        if (SPEECHIFY_API_KEY == null)
            Debug.LogWarning(DEBUG_PREFIX + "Warning: TTS API key is empty, check API Key File!");


        sfVoice = selectVoice.ToString().Substring(3).ToLower();
        sfModel = "simba-" + selectModel.ToString().Substring(1);
        Debug.Log("You have selected voice " + sfVoice + " and model " + sfModel);
    }


    public void Say(string textInput)
    {
        StartCoroutine(PlayTTS(textInput));
    }


    IEnumerator PlayTTS(string mesg)
    {
        TextToSpeechRequest ttsData = new TextToSpeechRequest
        {
            input = SimpleCleanText(mesg),
            voice_id = sfVoice,
            model = sfModel,
            audio_format = "mp3"
        };
        string jsonBody = JsonUtility.ToJson(ttsData);

        if (debug) Debug.Log(DEBUG_PREFIX + "Request: " + jsonBody);

        UnityWebRequest request = new UnityWebRequest(TTS_API_URI, "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("content-type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + SPEECHIFY_API_KEY);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(DEBUG_PREFIX + "TTS failed: " + request.error + " | " + request.downloadHandler.text);
            yield break;
        }

        // Response is JSON with base64-encoded audio_data
        SpeechifyResponse resp = JsonUtility.FromJson<SpeechifyResponse>(request.downloadHandler.text);
        if (resp == null || string.IsNullOrEmpty(resp.audio_data))
        {
            Debug.LogError(DEBUG_PREFIX + "Empty audio_data in response: " + request.downloadHandler.text);
            yield break;
        }

        // Write mp3 to temp file then load as AudioClip
        byte[] mp3Bytes = Convert.FromBase64String(resp.audio_data);
        string tmpPath = System.IO.Path.Combine(Application.temporaryCachePath, "tts_out.mp3");
        System.IO.File.WriteAllBytes(tmpPath, mp3Bytes);

        UnityWebRequest audioReq = UnityWebRequestMultimedia.GetAudioClip("file://" + tmpPath, AudioType.MPEG);
        yield return audioReq.SendWebRequest();

        if (audioReq.result == UnityWebRequest.Result.Success)
        {
            if (avtAnimator) avtAnimator.SetBool("isTalking", true);
            AudioClip clip = DownloadHandlerAudioClip.GetContent(audioReq);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(WaitForTalkingFinished());
        }
        else Debug.LogError(DEBUG_PREFIX + "Audio load failed: " + audioReq.error);
    }


    IEnumerator WaitForTalkingFinished()
    {
        while (GetComponent<AudioSource>().isPlaying)
            yield return null;
        if (avtAnimator) avtAnimator.SetBool("isTalking", false);
    }


    [Serializable]
    public class TextToSpeechRequest
    {
        public string input;
        public string voice_id;
        public string model;
        public string audio_format;
    }

    [Serializable]
    public class SpeechifyResponse
    {
        public string audio_data;   // base64-encoded mp3
    }


    string SimpleCleanText(string msg)
    {
        string result = "";
        for (int i = 0; i < msg.Length; i++)
        {
            switch (msg[i])
            {
                case '+': result += " plus ";    break;
                case ':': result += ", ";        break;
                case '*': result += ", ";        break;
                case '=': result += " equals ";  break;
                case '-': result += " ";         break;
                case '#': result += " hash ";    break;
                case '&': result += " and ";     break;
                case 'I':
                    if (i + 2 < msg.Length && msg[i + 1] == '\'' && msg[i + 2] == 'm')
                    {
                        result += "I am";
                        i += 2;
                    }
                    else result += 'I';
                    break;
                default:  result += msg[i];      break;
            }
        }
        return result;
    }
}
