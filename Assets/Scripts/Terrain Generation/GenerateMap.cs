using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour {

    public GameObject Map;

    public int HighElevation_Frequency;
    public BlockType.Type HighElevation;
    public int MediumElevation_Frequency;
    public BlockType.Type MediumElevation;
    public int LowElevation_Frequency;
    public BlockType.Type LowElevation;

    public bool generateDepthBlocks;

    public int map_xWidth;
    public int map_zWidth;

    public float seed;
    public float scale;
    public float smoothing;
    public int depthScale;

    public int[,] perlinValues;
    public int[,] heightValues;
    public GameObject[,] surfaceBlocks;
    public List<GameObject> depthBlocks;

    private int total;
    private int average;

    void Start() {
        surfaceBlocks = null;
        Generate();
    }

    public void ClearBlocks() {
        for (int x = 0; x < map_xWidth; x++) {
            for (int z = 0; z < map_zWidth; z++) {
                Destroy(surfaceBlocks[x, z]);
            }
        }
        foreach(GameObject depthBlock in depthBlocks) {
            Destroy(depthBlock);
        }
        surfaceBlocks = null;
        heightValues = null;
        perlinValues = null;
        total = 0;
        average = 0;
    }

    public void Generate() {
        if (surfaceBlocks != null) {
            ClearBlocks();
        }
        perlinValues = new int[map_xWidth, map_zWidth];
        for (int x = 0; x < map_xWidth; x++) {
            for (int z = 0; z < map_zWidth; z++) {
                float xModified = x / scale;
                float zModified = z / scale;
                int perlinValue = Mathf.FloorToInt(100 * Mathf.PerlinNoise(xModified + seed, zModified + seed));
                perlinValues[x, z] = perlinValue;
                total += perlinValue;
            }
        }
        average = total / (map_xWidth * map_zWidth);
        heightValues = new int[map_xWidth, map_zWidth];
        surfaceBlocks = new GameObject[map_xWidth, map_zWidth];
        float min = 0;
        float max = 1;
        float heightAverage = 0;
        for (int x = 0; x < map_xWidth; x++) {
            for (int z = 0; z < map_zWidth; z++) {
                int heightValue = 0;
                int perlinValue = perlinValues[x, z];
                bool polarity = (perlinValue - average) > 0;
                int difference = Mathf.Abs(perlinValue - average);
                float percentageDifference = (difference / smoothing) * 100;
                if (percentageDifference <= 25) {
                    heightValue = 0;
                } else {
                    if (polarity) {
                        heightValue = Mathf.FloorToInt(difference / smoothing);
                    } else {
                        heightValue = -Mathf.FloorToInt(difference / smoothing);
                    }
                }

                if (heightValue < min) {
                    min = heightValue;
                }

                if (heightValue > max) {
                    max = heightValue;
                }
                heightValues[x, z] = heightValue;
            }
        }
        heightAverage = (min + max) / 2f;
        GameObject highElevation = BlockType.GetBlock(HighElevation.ToString());
        GameObject mediumElevation = BlockType.GetBlock(MediumElevation.ToString());
        GameObject lowElevation = BlockType.GetBlock(LowElevation.ToString());
        for (int x = 0; x < map_xWidth; x++) {
            for (int z = 0; z < map_zWidth; z++) {
                
                GameObject block;
                if (heightValues[x,z] >= (max - HighElevation_Frequency)) {
                    block = Instantiate(highElevation);
                } else if (heightValues[x,z] >= (heightAverage - MediumElevation_Frequency) && 
                            heightValues[x,z] <= (heightAverage + MediumElevation_Frequency)) {
                    block = Instantiate(mediumElevation);
                } else if (heightValues[x,z] <= (min + LowElevation_Frequency)) {
                    block = Instantiate(lowElevation);
                } else {
                    block = Instantiate(BlockType.Grass);
                }
                block.name = (block.name).Substring(0, (block.name).Length - 7);
                block.transform.SetParent(Map.transform);
                block.transform.localPosition = new Vector3(x, heightValues[x,z], z);
                surfaceBlocks[x, z] = block;

                if (generateDepthBlocks) {
                    int yHeight = (int)(block.transform.localPosition.y);
                    yHeight -= 1;
                    while (yHeight > (min-depthScale)) {
                        GameObject dirt = Instantiate(BlockType.Dirt);
                        dirt.transform.SetParent(Map.transform);
                        dirt.transform.localPosition = new Vector3(x, yHeight, z);
                        depthBlocks.Add(dirt);
                        yHeight -= 1;
                    }
                }

            }
        }
    }
	
}
