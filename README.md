What is this project
====================
A Unity 6 AI voice pipeline that chains Speech-to-Text, LLM, and Text-to-Speech cloud services together.
Speak into a microphone, get a spoken AI response back.

Pipeline
========
Microphone -> STT -> AI_STT_Text_Filter -> LLM -> TTS -> AudioSource

Supported Providers
===================
Speech to Text (STT):
  - GroqCloud (Whisper large-v3 / turbo / distil)
  - HuggingFace (OpenAI-compatible endpoint)
  - ElevenLabs

LLM:
  - GroqCloud (Llama, Gemma, Mistral, DeepSeek, etc.)
  - Google Gemini (Flash 2.0 / 2.5)
  - Ollama (local models)
  - RAG context support via MariaDB/MySQL

Text to Speech (TTS):
  - Speechify (Simba models)
  - OpenAI TTS (via RapidAPI)
  - Speach (via RapidAPI)
  - ElevenLabs


Getting Started
===============
1. Pull/fork the repo
2. Open the project in Unity 6
3. Open Assets/Scenes/AITestScene.unity
4. Create the API keys file:
   - Create folder: Assets/Resources/Secure/
   - Create file:   Assets/Resources/Secure/APIKeys.txt
   - This folder is excluded from git (.gitignore)

5. Add your keys to APIKeys.txt using this format (one per line):
      Groq_API_Key: your-key-here
      Speechify_API_Key: your-key-here
      Google_API_Key: your-key-here
      HF_API_Key: your-key-here
      ElevenLabs_API_Key: your-key-here
      Rapid_API_Key: your-key-here

6. In the scene, select the AIManager GameObject and configure the
   AI Orchestrator component in the Inspector:
   - Assign the STT, LLM, and TTS components you want to use
   - Only assign ONE component per category to avoid duplicate responses

7. Set the File Path field on the API Keys component to: Secure/APIKeys

8. Hit Play. Hold Spacebar to record, release to send.

Free Tier API Keys
==================
- GroqCloud:    free tier available at console.groq.com
- Google Gemini: free tier available at aistudio.google.com
- HuggingFace:  free tier available at huggingface.co
- Speechify:    paid service at speechify.com
- ElevenLabs:   free tier available at elevenlabs.io
- Ollama:       fully local, no key needed

Project Structure
=================
Assets/Scripts/AI/
  AI_Orchestrator.cs      - Central hub, routes between all components
  AI_STT_Text_Filter.cs   - Routes STT output to LLM
  AI_WAV.cs               - Converts AudioClip to WAV for STT APIs
  API_Keys.cs             - Loads API keys from the secure file
  STT_Groq_OpenAI.cs      - Groq Whisper STT
  STT_HF_OpenAI.cs        - HuggingFace STT
  STT_11_Labs.cs          - ElevenLabs STT
  LLM_Groq.cs             - GroqCloud LLM
  LLM_Google.cs           - Google Gemini LLM
  LLM_Ollama.cs           - Ollama local LLM
  RAG_MariaDB.cs          - RAG context retrieval (optional)
  TTS_SF_Simba.cs         - Speechify TTS
  TTS_RA_OpenAI.cs        - OpenAI TTS via RapidAPI
  TTS_RA_Speach.cs        - Speach via RapidAPI
  TTS_11_Labs.cs          - ElevenLabs TTS
