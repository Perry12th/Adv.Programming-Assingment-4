using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode { Easy, Medium, Hard };
public enum PlayerSkill { Rookie, Skilled, Master };
public class PuzzlePipeManager : MonoBehaviour
{
    public static PuzzlePipeManager instance;
    [SerializeField]
    private List<GameObject> pipes = new List<GameObject>();
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject bombPipePrefab;
    [SerializeField]
    private GameObject entryPipePrefab;
    [SerializeField]
    private GameObject exitPipePrefab;
    [SerializeField]
    public int boardSizeX;
    [SerializeField]
    public int boardSizeY;
    private GameObject[,] tiles;
    [SerializeField]
    public Mode gameMode = Mode.Easy;
    [SerializeField]
    public PlayerSkill playerSkill = PlayerSkill.Rookie;
    public PipeController activePipe;
    public LayerMask pipeLayer;
    public float checkDistance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void GameOver()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartGame()
    {
        Vector2 offset = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

    private void CreateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[boardSizeX, boardSizeY];

        float startPointX = transform.position.x;
        float startPointY = transform.position.y;

        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(startPointX + (xOffset * x), startPointY + (yOffset * y), transform.position.z), tilePrefab.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<GameObject> possiblePipes = new List<GameObject>();
                possiblePipes.AddRange(pipes);
                if (gameMode != Mode.Easy)
                {
                    possiblePipes.Add(bombPipePrefab);
                }

                GameObject newPipe = Instantiate(possiblePipes[Random.Range(0, possiblePipes.Count)], newTile.transform);
                newPipe.transform.parent = newPipe.transform;
                newTile.GetComponent<TileController>().pipe = newPipe.GetComponent<PipeController>();
            }
        }

        // Add in the entry and exit pipes
        Vector3 entryPipeLocation = tiles[0, Random.Range(0, boardSizeY - 1)].transform.position;
        entryPipeLocation.x -= entryPipePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        GameObject entryPipe = Instantiate(entryPipePrefab, entryPipeLocation, entryPipePrefab.transform.rotation);
        entryPipe.transform.parent = transform;

        Vector3 exitPipeLocation = tiles[boardSizeX - 1, Random.Range(0, boardSizeY - 1)].transform.position;
        exitPipeLocation.x += exitPipePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        GameObject exitPipe = Instantiate(exitPipePrefab, exitPipeLocation, exitPipePrefab.transform.rotation);
        exitPipe.transform.parent = transform;

        activePipe = entryPipe.GetComponent<PipeController>();
    }
}
