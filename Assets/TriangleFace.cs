using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TriangleFace
{
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public TriangleFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        
        if (localUp.z != 0)
        {
            axisA = new Vector3(-localUp.z, localUp.x, localUp.y);
        }
        else
        {
            axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        }

        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[((resolution * (resolution - 1)) / 2) + resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 3];
        int i = 0;
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for(int x = 0; x < y + 1; x++)
            {
                float percentX;
                float percentY;
                
                if (y == 0)
                {
                    percentX = .5f;
                    percentY = 0f;
                } 
                else
                {
                    percentX = x / (y * 1f);
                    percentY = y / ((resolution - 1) * 1.0f);
                }

                Vector3 pointOnFace = localUp + ((percentX - .5f) * 2 * percentY * axisA) + ((percentY - .5f) * 2 * axisB);
                pointOnFace = Quaternion.AngleAxis(30, axisA) * (pointOnFace - (localUp + axisB)) + (localUp + axisB);
                vertices[i] = pointOnFace;

                if (y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + y + 2;
                    triangles[triIndex + 2] = i + y + 1;
                    triIndex += 3;

                    if (x != y)
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + 1;
                        triangles[triIndex + 2] = i + y + 2;
                        triIndex += 3;
                    }
                }

                i++;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
