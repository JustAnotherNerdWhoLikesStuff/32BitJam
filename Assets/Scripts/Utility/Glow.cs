using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour
{
    public float GlowSize_percentIncrease;
    public Material GlowMaterial;
    public Shader GlowShader;

    public bool Glowing
    {
        get { return glowing; }
        set
        {
            glowing = value;
            glowMeshRenderer.enabled = glowing;
        }
    }

    private bool glowing = false;
    private GameObject glowObject;
    private Mesh glowMesh;
    private MeshFilter glowMeshFilter;
    private MeshRenderer glowMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        float scaleFactor = 1.0f + GlowSize_percentIncrease / 100.0f;

        glowObject = new GameObject("Glow object");
        glowObject.transform.parent = gameObject.transform;
        glowObject.transform.position = gameObject.transform.position;
        glowObject.transform.rotation = gameObject.transform.rotation;

        glowMeshRenderer = glowObject.AddComponent<MeshRenderer>();
        GlowMaterial.shader = GlowShader;
        glowMeshRenderer.sharedMaterial = GlowMaterial;
        glowMeshRenderer.enabled = false;

        glowMeshFilter = glowObject.AddComponent<MeshFilter>();

        glowMesh = new Mesh();

        MeshFilter gameObjectMeshFilter = new MeshFilter();
        Mesh gameObjectMesh = new Mesh();

        if (gameObject.TryGetComponent<MeshFilter>(out gameObjectMeshFilter))
        {
            glowObject.transform.localScale = gameObject.transform.localScale * scaleFactor;
            gameObjectMesh = gameObjectMeshFilter.mesh;
        }
        else
        {
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTransforms.Length; i++)
            {
                childTransforms[i].localScale *= scaleFactor;
            }

            MeshFilter[] childMeshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] meshCombo = new CombineInstance[childMeshFilters.Length - 1];
            for (int i = 0; i < meshCombo.Length; i++)
            {
                meshCombo[i].transform = gameObject.transform.worldToLocalMatrix * childMeshFilters[i].transform.localToWorldMatrix;
                meshCombo[i].mesh = childMeshFilters[i].sharedMesh;
            }
            gameObjectMesh.CombineMeshes(meshCombo);

            for (int i = 0; i < childTransforms.Length; i++)
            {
                childTransforms[i].localScale *= (1.0f / scaleFactor);
            }
        }

        // Copy vertices
        glowMesh.vertices = gameObjectMesh.vertices;

        // Invert normals
        glowMesh.normals = gameObjectMesh.normals;
        for (int i = 0; i < gameObjectMesh.normals.Length; i++)
        {
            // Invert the normals to acheive outline effect.
            glowMesh.normals[i] = -gameObjectMesh.normals[i];
        }

        // Swap triangles
        glowMesh.triangles = gameObjectMesh.triangles;
        for (int s = 0; s < gameObjectMesh.subMeshCount; s++)
        {
            int[] originalTriangles = gameObjectMesh.GetTriangles(s);
            int[] glowTriangles = glowMesh.GetTriangles(s);
            for (int i = 0; i < originalTriangles.Length; i += 3)
            {
                glowTriangles[i] = originalTriangles[i + 1];
                glowTriangles[i + 1] = originalTriangles[i];
            }
            glowMesh.SetTriangles(glowTriangles, s);
        }

        // Skip texture; we need solid color

        glowMeshFilter.mesh = glowMesh;
        glowObject.transform.localScale /= 2.0f;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}