using UnityEngine;

public class LipSyncController : MonoBehaviour
{
    public Animator animator;
    public SkinnedMeshRenderer faceMesh;

    public string blendShapeName = "jawOpen";

    public float talkingValue = 70f;
    public float smoothSpeed = 10f;

    private int blendShapeIndex;
    private float currentWeight;

    void Start()
    {
        blendShapeIndex =
            faceMesh.sharedMesh.GetBlendShapeIndex(blendShapeName);

        Debug.Log("BlendShape Index = " + blendShapeIndex);
    }

    void Update()
    {
        if (animator == null || faceMesh == null)
            return;

        bool isTalking = animator.GetBool("isTalking");

        float jawTarget = 0f;
        float smileTarget = 0f;

        if (isTalking)
        {
            float noise =
                Mathf.PerlinNoise(Time.time * 10f, 0f);

            jawTarget = noise * talkingValue;

            smileTarget =
                Mathf.PerlinNoise(Time.time * 6f, 1f) * 15f;
        }

        currentWeight = Mathf.Lerp(
            currentWeight,
            jawTarget,
            Time.deltaTime * smoothSpeed
        );

        faceMesh.SetBlendShapeWeight(
            faceMesh.sharedMesh.GetBlendShapeIndex("jawOpen"),
            currentWeight
        );

        faceMesh.SetBlendShapeWeight(
            faceMesh.sharedMesh.GetBlendShapeIndex("mouthSmileLeft"),
            smileTarget
        );

        faceMesh.SetBlendShapeWeight(
            faceMesh.sharedMesh.GetBlendShapeIndex("mouthSmileRight"),
            smileTarget
        );
    }
}