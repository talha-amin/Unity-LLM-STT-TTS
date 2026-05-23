using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Loads patient data from a JSON TextAsset and formats it as plain text
/// to be injected into the LLM system prompt as static context.
///
/// HOW TO USE:
///   1. Add this component to the same GameObject as AI_Orchestrator.
///   2. Drag Assets/Data/PatientData.json into the "Patient Data File" field in the Inspector.
///   3. The AI_Orchestrator will call GetFormattedContext() automatically before LLM Init().
/// </summary>
public class PatientData_Loader : MonoBehaviour
{
    [SerializeField] private TextAsset patientDataFile;

    [SerializeField] private bool debug = false;
    const string DEBUG_PREFIX = "PATIENT_LOADER: ";

    private PatientDatabase database;


    /// <summary>
    /// Call this before LLM Init() to parse the JSON.
    /// Returns true if data was loaded successfully.
    /// </summary>
    public bool Load()
    {
        if (patientDataFile == null)
        {
            Debug.LogError(DEBUG_PREFIX + "No patient data file assigned! Drag PatientData.json into the Inspector.");
            return false;
        }

        try
        {
            database = JsonUtility.FromJson<PatientDatabase>(patientDataFile.text);
        }
        catch (Exception e)
        {
            Debug.LogError(DEBUG_PREFIX + "Failed to parse patient data JSON: " + e.Message);
            return false;
        }

        if (database == null || database.patients == null || database.patients.Length == 0)
        {
            Debug.LogWarning(DEBUG_PREFIX + "Patient data file is empty or malformed.");
            return false;
        }

        if (debug)
            Debug.Log(DEBUG_PREFIX + "Loaded " + database.patients.Length + " patient(s) successfully.");

        return true;
    }


    /// <summary>
    /// Returns all patients formatted as readable plain text for injection into the LLM system prompt.
    /// Call Load() first.
    /// </summary>
    public string GetFormattedContext()
    {
        if (database == null || database.patients == null)
        {
            Debug.LogWarning(DEBUG_PREFIX + "No data loaded. Call Load() first.");
            return "";
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("You have access to the following patient records in ISBAR format. Use this data to answer any questions about these patients accurately.");
        sb.AppendLine();

        foreach (Patient p in database.patients)
        {
            sb.AppendLine("------------------------------------------------------------");
            sb.AppendLine("PATIENT RECORD");
            sb.AppendLine("------------------------------------------------------------");
            sb.AppendLine("IDENTIFY:");
            sb.AppendLine("  Name       : " + p.name);
            sb.AppendLine("  DOB        : " + p.dob);
            sb.AppendLine("  MRN        : " + p.mrn);
            sb.AppendLine("  Location   : " + p.ward);
            sb.AppendLine("  Consultant : " + p.consultant);
            sb.AppendLine();
            sb.AppendLine("SITUATION:");
            sb.AppendLine("  " + p.situation);
            sb.AppendLine();
            sb.AppendLine("BACKGROUND:");
            sb.AppendLine("  " + p.background);
            sb.AppendLine();
            sb.AppendLine("ASSESSMENT:");
            sb.AppendLine("  " + p.assessment);
            sb.AppendLine();
            sb.AppendLine("RECOMMENDATION:");
            sb.AppendLine("  " + p.recommendation);
            sb.AppendLine();
        }

        sb.AppendLine("------------------------------------------------------------");

        string result = sb.ToString();

        if (debug)
            Debug.Log(DEBUG_PREFIX + "Formatted context:\n" + result);

        return result;
    }


    // ====================================================
    // JSON data structures — must match PatientData.json
    // ====================================================

    [Serializable]
    public class Patient
    {
        public string id;
        public string name;
        public string dob;
        public string mrn;
        public string ward;
        public string consultant;
        public string situation;
        public string background;
        public string assessment;
        public string recommendation;
    }

    [Serializable]
    public class PatientDatabase
    {
        public Patient[] patients;
    }
}
