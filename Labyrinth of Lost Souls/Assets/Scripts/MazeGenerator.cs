using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;

    private bool[,] maze;

    void Start()
    {
        GenerateMaze();
        BuildMaze();
    }

    // Genera la estructura del laberinto usando el algoritmo de backtracking
    private void GenerateMaze()
    {
        maze = new bool[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = false;

        Backtrack(0, 0);
    }

    // Construye el laberinto en la escena instanciando prefabs
    private void BuildMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(maze[x, y] ? floorPrefab : wallPrefab, position, Quaternion.identity, transform);
            }
        }
    }

    // Algoritmo de backtracking para generar el laberinto
    private void Backtrack(int x, int y)
    {
        maze[x, y] = true;

        foreach (Vector2 direction in GetShuffledDirections())
        {
            int newX = x + (int)direction.x * 2;
            int newY = y + (int)direction.y * 2;

            if (IsInside(newX, newY) && !maze[newX, newY])
            {
                maze[x + (int)direction.x, y + (int)direction.y] = true;
                Backtrack(newX, newY);
            }
        }
    }

    // Verifica si las coordenadas están dentro del rango del laberinto
    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    // Obtiene direcciones aleatorias para el algoritmo
    private Vector2[] GetShuffledDirections()
    {
        Vector2[] directions = {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        for (int i = directions.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2 temp = directions[i];
            directions[i] = directions[j];
            directions[j] = temp;
        }

        return directions;
    }
}
