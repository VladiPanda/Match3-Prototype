using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    /// <summary>
    /// Tiles
    /// </summary>
    public int width;
    public int height;
    public GameObject bgTilePrefab;
    /// <summary>
    /// Gems
    /// </summary>
    public Gem[] gems;


    void Start()
    {
        Setup();
    }
    /// <summary>
    /// Create BG
    /// </summary>
    private void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, position, Quaternion.identity);
                bgTile.transform.parent = transform; // attaching bricks in main tile
                bgTile.name = "BG Tile - " + x + ", " + y; // for more correct naming tiles

                int gemToUse = Random.Range(0, gems.Length);

                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
            }
        }
    }

    private void SpawnGem(Vector2Int position, Gem gemToSpawn)
    {
        Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = "Gem - " + position.x + ", " + position.y;
    }
}
