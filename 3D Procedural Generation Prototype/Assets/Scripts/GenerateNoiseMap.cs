using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNoiseMap : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Class built to generate a Perlin Noise Array

    public float[,] GeneratePerlinNoiseMap(int depth, int width, float scale)
    {
        float[,] noiseMap = new float[depth, width];

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {

                float x = j / scale;
                float z = i / scale;

                noiseMap[i,j] = Mathf.PerlinNoise(x, z);


            }
        }

        return noiseMap;
    }

}
