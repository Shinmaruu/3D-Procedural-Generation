using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileGeneration : MonoBehaviour
{

    [SerializeField] GenerateNoiseMap noiseMapGenerator;
    [SerializeField] MeshRenderer tileRenderer;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] float mapScale;
    [SerializeField] float heightMultiplier;

    [SerializeField] TerrainType[] terrainTypes;


    void Start()
    {
        GenerateTile();
    }

    void Update()
    {
        
    }

    void GenerateTile()
    {
        Vector3[] meshVerticies = this.meshFilter.mesh.vertices;

        int tileDepth = (int)Mathf.Sqrt(meshVerticies.Length);
        int tileWidth = tileDepth;

        float[,] heightMap = this.noiseMapGenerator.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale);

        Texture2D tileTexture = BuildTexture(heightMap);
        MeshVerticies(heightMap);
        this.tileRenderer.material.mainTexture = tileTexture;
    }

    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int i = 0; i < tileDepth; i++)
        {
            for (int j = 0; j < tileWidth; j++)
            {
                int colorIndex = i * tileWidth + j;
                float height = heightMap[i, j];
                TerrainType terrainType = ChooseTerrainType(height);
                colorMap[colorIndex] = terrainType.color;
            }
        }
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();

        return tileTexture;
    }

    TerrainType ChooseTerrainType(float height)
    {
        foreach (TerrainType terrainType in terrainTypes)
        {
            if (height < terrainType.height)
            {
                return terrainType;
            }
        }
        return terrainTypes[terrainTypes.Length - 1];
    }

    private void MeshVerticies(float[,] heightMap)
    {

        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int vertexIndex = 0;
        for (int i = 0; i < tileDepth; i++)
        {
            for (int j = 0; j < tileWidth; j++)
            {
                float height = heightMap[i, j];

                Vector3 vertex = meshVertices[vertexIndex];
                meshVertices[vertexIndex] = new Vector3(vertex.x, height * this.heightMultiplier, vertex.z);

                vertexIndex++;
            }
        }
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
        this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }


}




[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color color;
}
