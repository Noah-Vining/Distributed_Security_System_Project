using UnityEngine;
using System.Collections;

public class MeshWave : MonoBehaviour
{
    float scale = 0.1f;
    float speed = 1.0f;
    float noiseStrength = 1f;
    float noiseWalk = 1f;

    private Vector3[] baseHeight;

    void Update()
    {
        waveGen();
    }

    public void waveGen()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;

        Vector3[] vertices = new Vector3[baseHeight.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];

            vertex.z += Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
            vertex.z += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}