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
    public Gem[,] allGems; // will store x&y value for each gem
    public float gemSpeed;
    /// <summary>
    /// Matching
    /// </summary>
    [HideInInspector]
    public MatchFinder matchFind;

    private void Awake()
    {
        matchFind = FindObjectOfType<MatchFinder>();
    }
    void Start()
    {
        allGems = new Gem[width, height];

        Setup();
    }

    private void Update()
    {
        matchFind.FindAllMatches();
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

                int iterations = 0; // crutch to prevent potential crash
                while (MatchesAt(new Vector2Int(x,y), gems[gemToUse]) && iterations < 100)
                {
                    gemToUse = Random.Range(0, gems.Length);
                    iterations++;
                }

                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
            }
        }
    }

    private void SpawnGem(Vector2Int position, Gem gemToSpawn)
    {
        Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = "Gem - " + position.x + ", " + position.y;
        allGems[position.x, position.y] = gem;

        gem.SetupGem(position, this); // this means we will use current board
    }
    /// <summary>
    /// MatchesGem
    /// </summary>
    /// <param name="positionToCheck"></param>
    /// <param name="gemToCheck"></param>
    /// <returns></returns>
    bool MatchesAt(Vector2Int positionToCheck, Gem gemToCheck)
    {
        if(positionToCheck.x > 1)
        {
            if(allGems[positionToCheck.x - 1, positionToCheck.y].type == gemToCheck.type && allGems[positionToCheck.x - 2, positionToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }

        if (positionToCheck.y > 1)
        {
            if (allGems[positionToCheck.x, positionToCheck.y - 1].type == gemToCheck.type && allGems[positionToCheck.x, positionToCheck.y - 2].type == gemToCheck.type)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// DestroyingGems
    /// </summary>
    /// <param name="position"></param>
    private void DestroyMatchedGemAt(Vector2Int position)
    {
        if(allGems[position.x, position.y] != null) // checking all gems
        {
            if (allGems[position.x, position.y].isMatched) // doublecheck
            {
                Destroy(allGems[position.x, position.y].gameObject);
                allGems[position.x, position.y] = null;
            }
        }
          
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < matchFind.currentMatches.Count; i++)
        {
            if(matchFind.currentMatches[i] != null) // checking list
            {
                DestroyMatchedGemAt(matchFind.currentMatches[i].positionIndex);
            }
        }
    }
}
