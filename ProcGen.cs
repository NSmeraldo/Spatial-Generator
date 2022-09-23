using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProcGen : MonoBehaviour
{
    public List<Biome> biomes;
    public GameObject[] terrainPrefab;
    [SerializeField] private int width = 36;
    [SerializeField] private int height = 11;
    [SerializeField] private int upheight = 10;
    private GameObject[,] terrainList;
    private bool isDirty = true;
    public float offsetX;
    public float offsetY;
    [SerializeField] private float perlinWeight = 0.01f;
    [SerializeField] private float step = 0.2f;
    private float minHeight;
    bool setminHeight = false;
    float xRandomOffset;
    float yRandomOffset;
    
    


    void Awake()
    {
        //Random.InitState(1);
        xRandomOffset = Random.value;
        yRandomOffset = Random.value;
        Debug.Log("Setting up Terrain");
        terrainList = new GameObject[width, height];
        SetTerrainLocAndType();

    }

    void SetTerrainLocAndType()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {


                float yDiff = y * perlinWeight + offsetX;
                float xDiff = x * perlinWeight + offsetY;


                if (setminHeight == false)
                {
                    minHeight = yDiff;
                    setminHeight = true;
                }
                if (yDiff < minHeight)
                {
                    minHeight = yDiff;
                }


                float noise = Mathf.PerlinNoise(xDiff + xRandomOffset, yDiff + yRandomOffset);
                float loc = Mathf.Round(noise * 25);

                Vector3 pos = transform.position + new Vector3(x, loc, y);
                int prefabSelected = 0;
                for(int a = 0; a< biomes.Count; a++)
                {
                    Collider[] colliders;
                    colliders = Physics.OverlapSphere(pos, 0.2f);
                    if (colliders.Contains(biomes[a].col))
                    {
                        prefabSelected = (int)(noise * biomes[a].biomecat.terrainPrefab.Length);
                        prefabSelected = Mathf.Clamp(prefabSelected, 0, biomes[a].biomecat.terrainPrefab.Length - 1);
                        Debug.Log(prefabSelected);

                        terrainList[x, y] = Instantiate(biomes[a].biomecat.terrainPrefab[prefabSelected], pos, transform.rotation, transform);
                        terrainList[x, y].name = "Tile " + x + "," + y;
                        /*for (int z = 1; z < Mathf.Abs(Mathf.Abs(minHeight) - Mathf.Abs(loc)); z++)
                        {
                            Debug.Log("ydiff =" + yDiff);
                            Debug.Log(minHeight);
                            Vector3 pos2 = transform.position + new Vector3(x, loc - z, y);
                            terrainList[x, y] = Instantiate(biomes[a].biomecat.terrainPrefab[prefabSelected], pos2, transform.rotation, transform);
                        }*/
                    }
                    else
                    {
                        prefabSelected = (int)(noise * biomes[1].biomecat.terrainPrefab.Length);
                        prefabSelected = Mathf.Clamp(prefabSelected, 0, biomes[1].biomecat.terrainPrefab.Length - 1);
                        Debug.Log(prefabSelected);

                        terrainList[x, y] = Instantiate(biomes[1].biomecat.terrainPrefab[prefabSelected], pos, transform.rotation, transform);
                        terrainList[x, y].name = "Tile " + x + "," + y;
                        /*for (int z = 1; z < Mathf.Abs(Mathf.Abs(minHeight) - Mathf.Abs(loc)); z++)
                        {
                            Debug.Log("ydiff =" + yDiff);
                            Debug.Log(minHeight);
                            Vector3 pos2 = transform.position + new Vector3(x, loc - z, y);
                            terrainList[x, y] = Instantiate(biomes[1].biomecat.terrainPrefab[prefabSelected], pos2, transform.rotation, transform);
                        }*/
                    }
                }
            }
        }
        isDirty = false;
    }

}