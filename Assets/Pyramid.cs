using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    [Range(2,256)]
    public int resolution = 8;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TriangleFace[] triangleFaces;
    SquareFace squareFace;

    private void OnValidate()
    {
        Initalize();
        GenerateMesh();
    }

    void Initalize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[5];
        }
        
        triangleFaces = new TriangleFace[4];

        Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left, Vector3.down };

        for (int i = 0; i < 5; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }           
        }

        for (int i = 0; i < triangleFaces.Length; i++)
        {
            triangleFaces[i] = new TriangleFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }

        squareFace = new SquareFace(meshFilters[4].sharedMesh, resolution, directions[4]);
    }

    void GenerateMesh()
    {
        foreach(TriangleFace face in triangleFaces)
        {
            face.ConstructMesh();
        }

        squareFace.ConstructMesh();
    }
}
