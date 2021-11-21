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
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, position, Quaternion.identity);
                bgTile.transform.parent = transform; // attaching bricks in main tile
                bgTile.name = "BG Tile - " + x + ", " + y; // for more correct naming tiles

                int gemToUse = Random.Range(0, gems.Length);

                int iterations = 0; // crutch to prevent potential crash
                while (MatchesAt(new Vector2Int(x, y), gems[gemToUse]) && iterations < 100)
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
        if (positionToCheck.x > 1)
        {
            if (allGems[positionToCheck.x - 1, positionToCheck.y].type == gemToCheck.type && allGems[positionToCheck.x - 2, positionToCheck.y].type == gemToCheck.type)
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
        if (allGems[position.x, position.y] != null) // checking all gems
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
        for (int i = 0; i < matchFind.currentMatches.Count; i++)
        {
            if (matchFind.currentMatches[i] != null) // checking list
            {
                DestroyMatchedGemAt(matchFind.currentMatches[i].positionIndex);
            }
        }

        StartCoroutine(DecreaseRowCoroutine());
    }
    /// <summary>
    /// FallingGems
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseRowCoroutine()
    {
        yield return new WaitForSeconds(.2f); // delay before explosion gems

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    allGems[x, y].positionIndex.y -= nullCounter;
                    allGems[x, y - nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }

            nullCounter = 0;
        }

        StartCoroutine(FillBoardCoroutine());
    }
    /// <summary>
    /// RefillingGems
    /// </summary>
    /// <returns></returns>
    private IEnumerator FillBoardCoroutine() // refilling/matching logic, cascade effect
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();

        yield return new WaitForSeconds(.5f);

        matchFind.FindAllMatches();

        if(matchFind.currentMatches.Count > 0)
        {
            yield return new WaitForSeconds(1.5f);
            DestroyMatches();
        }
    }

    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(allGems[x,y] == null) 
                { 
                int gemToUse = Random.Range(0, gems.Length);

                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }
            }
        }

    }
}