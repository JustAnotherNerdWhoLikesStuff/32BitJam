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
        glowObject = new GameObject();
        glowObject.transform.parent = gameObject.transform;
        glowObject.transform.position = gameObject.transform.position;
        glowObject.transform.rotation = gameObject.transform.rotation;
        float scale = 1.0f + GlowSize_percentIncrease / 100.0f;
        glowObject.transform.localScale = new Vector3(scale, scale, scale);

        glowMeshRenderer = glowObject.AddComponent<MeshRenderer>();
        GlowMaterial.shader = GlowShader;
        glowMeshRenderer.sharedMaterial = GlowMaterial;
        glowMeshRenderer.enabled = false;

        glowMeshFilter = glowObject.AddComponent<MeshFilter>();

        glowMesh = new Mesh();

        Mesh gameObjectMesh = gameObject.GetComponent<MeshFilter>().mesh;

        glowMesh.vertices = gameObjectMesh.vertices;

        glowMesh.normals = gameObjectMesh.normals;
        for (int i = 0; i < gameObjectMesh.normals.Length; i++)
        {
            // Invert the normals to acheive outline effect.
            glowMesh.normals[i] = -gameObjectMesh.normals[i];
        }

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
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}