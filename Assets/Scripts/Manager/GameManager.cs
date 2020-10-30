using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MapGenerator MapGenerator;
    public GameObject PacmanPrefab;
    public GameObject GhostPrefab;
    private PacmanManager _pacmanManager;

    public bool IsGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        MapGenerator.Initialize();
        GameObject pacman = Instantiate(PacmanPrefab, MapGenerator.GetPacmanSpawnPosition(), Quaternion.identity);
        _pacmanManager = pacman.GetComponent<PacmanManager>();
        _pacmanManager.Initialize(MapGenerator.GetPacmanSpawnPositionIndex());

        for (int i = 0; i < MapGenerator.GetGhostSpawnPositions().Count; i++)
        {
            GameObject ghost = Instantiate(GhostPrefab, MapGenerator.GetGhostSpawnPositions()[i], Quaternion.identity);
            GhostManager ghostManager = ghost.GetComponent<GhostManager>();
            ghostManager.Initialize(_pacmanManager);
        }
    }

    private void Update()
    {
        if(MapGenerator.IsClear())
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public static Vector2Int GetIndexPosition(MapGenerator mapGenerator, Transform transform)
    {
        Vector2Int mapSize = mapGenerator.GetMapSize();
        int x = Mathf.RoundToInt(transform.position.x / mapGenerator.GetTileOffset()) + mapSize.x / 2;
        int y = Mathf.RoundToInt(transform.position.y / mapGenerator.GetTileOffset()) + mapSize.y / 2;
        return new Vector2Int(x, y);
    }

    public static Vector3 GetWorldPosition(MapGenerator mapGenerator, Vector2Int indexPos)
    {
        Vector2Int mapSize = mapGenerator.GetMapSize();
        indexPos.x = indexPos.x - (mapSize.x / 2);
        indexPos.y = indexPos.y - (mapSize.y / 2);

        return new Vector3(indexPos.x * mapGenerator.GetTileOffset(), indexPos.y * mapGenerator.GetTileOffset(), 0.0f);
    }
}
