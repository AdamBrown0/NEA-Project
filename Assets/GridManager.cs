using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class GridManager : MonoBehaviour
{
    public static bool superSecretSettings = false;

    public static int rows;
    public static int cols;
    public static int minPreyCount;
    public static int minPredatorCount;
    public static int maxPreyCount;
    public static int maxPredatorCount;


    private string[,] grid = new string[rows, cols];
    public object[,] entityGrid = new object[rows, cols];
    private GameObject[,] gameObjectGrid = new GameObject[rows, cols];


    private float tileSize = 1f;
    private float tileScale = 0f;

    SpriteRenderer PreyRenderer;
    SpriteRenderer PredatorRenderer;

    public static Transform GridManagerTransform;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AllocConsole();  

    // Start is called before the first frame update
    void Start()
    {
        //rows = first.rows;
        //cols = first.cols;
        tileScale = (float)5 / (float)rows;
        tileSize = tileScale;
        Debug.ClearDeveloperConsole();
        GenerateGrid();
        GenerateEntities();
        GridManagerTransform = transform;

        //giving Simulation class access to both grids
        Assets.Simulation.grid = grid;
        Assets.Simulation.entityGrid = entityGrid;
    }

    private void GenerateGrid()
    {
        //need to make minimum entity count because sometimes none spawn.

        //setting rules for generating non-grass tiles
        int barrierCount = 0;
        int maxBarrier = (rows * cols) / 5;
        int waterCount = 0;
        int maxWater = (rows * cols) / 5;

        //makes sure square
        //cols = 8 * (5 / rows);

        //makes scale nice
        //tileScale = (float)5 / (float) rows;
        //tileSize = tileScale;

        GameObject referenceTileWater;
        GameObject referenceTileGrass;
        GameObject referenceTileBarrier;
        //reference Tiles for ease of access later on
        if (superSecretSettings)
        {
            referenceTileWater = (GameObject)Instantiate(Resources.Load("AmongUsWater"));
            referenceTileGrass = (GameObject)Instantiate(Resources.Load("AmongUsGrass"));
            referenceTileBarrier = (GameObject)Instantiate(Resources.Load("AmongUsBarrier"));
        }
        else
        {
            referenceTileWater = (GameObject)Instantiate(Resources.Load("Water"));
            referenceTileGrass = (GameObject)Instantiate(Resources.Load("Grass"));
            referenceTileBarrier = (GameObject)Instantiate(Resources.Load("Barrier"));
        }

        


        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int random = UnityEngine.Random.Range(1, 6);
                Debug.Log($"Position = ({col}, {row}), random = {random}");

                int anotherRan = UnityEngine.Random.Range(1, 10);

                //create barrier
                if (random == 4)
                {
                    if (barrierCount < maxBarrier)
                    {
                        GameObject tileBarrier = (GameObject)Instantiate(referenceTileBarrier, transform);
                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        

                        tileBarrier.transform.localScale = new Vector2(tileScale, tileScale);
                        tileBarrier.transform.position = new Vector2(posX, posY);
                        tileBarrier.name = $"Barrier {posX} {posY}";
                        barrierCount += 1;

                        grid[row, col] = "B";
                    }
                    else
                    {
                        GameObject tileGrass = (GameObject)Instantiate(referenceTileGrass, transform);

                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        tileGrass.transform.localScale = new Vector2(tileScale, tileScale);
                        tileGrass.transform.position = new Vector2(posX, posY);
                        tileGrass.name = $"Grass {posX} {posY}";

                        grid[row, col] = "G";
                    }
                }
                //create water
                else if (random == 5)
                {
                    if (waterCount < maxWater)
                    {
                        GameObject tileWater = (GameObject)Instantiate(referenceTileWater, transform);
                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        tileWater.transform.localScale = new Vector2(tileScale, tileScale);
                        tileWater.transform.position = new Vector2(posX, posY);
                        tileWater.name = $"Water {posX} {posY}";
                        waterCount += 1;

                        grid[row, col] = "W";
                    }
                    else
                    {
                        GameObject tileGrass = (GameObject)Instantiate(referenceTileGrass, transform);

                        float posX = col * tileSize;
                        float posY = row * -tileSize;

                        tileGrass.transform.localScale = new Vector2(tileScale, tileScale);
                        tileGrass.transform.position = new Vector2(posX, posY);
                        tileGrass.name = $"Grass {posX} {posY}";

                        grid[row, col] = "G";
                    }
                }
                //create entities 
                //entities can only generate on grass tiles so grass tile is always created underneath
                else if (random == 1 || random == 2 || random == 3)
                {
                    GameObject tileGrass = (GameObject)Instantiate(referenceTileGrass, transform);

                    float posX = col * tileSize;
                    float posY = row * -tileSize;

                    tileGrass.transform.localScale = new Vector2(tileScale, tileScale);
                    tileGrass.transform.position = new Vector2(posX, posY);
                    tileGrass.name = $"Grass {posX} {posY}";

                    grid[row, col] = "G";
                }
                else
                {
                    Debug.Log($"random = {random}");
                }

                

            }
        }

        Destroy(referenceTileGrass);
        Destroy(referenceTileWater);
        Destroy(referenceTileBarrier);
        
        

        //camera
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
    }

    public void displayEntitesOnConsole(object[,] entityGridD)
    {
        Assets.ConsoleStuff.DisplayOnConsole display = new Assets.ConsoleStuff.DisplayOnConsole();
        display.displayEntitiesOnConsoleExternal(entityGridD);
    }

    public GameObject[,] destroyCurrentEntities(GameObject[,] gameObjectGridD)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Destroy(gameObjectGridD[row, col]);
                gameObjectGridD[row, col] = null;
            }
        }
        return gameObjectGridD;
    }

    public void displayEntities(object[,] entityGridD)
    {
        gameObjectGrid = destroyCurrentEntities(gameObjectGrid);
        GameObject referenceTilePredator;
        GameObject referenceTilePrey;
        if (superSecretSettings)
        {
            referenceTilePredator = (GameObject)Instantiate(Resources.Load("AmongUsPredator"));
            referenceTilePrey = (GameObject)Instantiate(Resources.Load("AmongUsPrey"));
        }
        else
        {
            referenceTilePredator = (GameObject)Instantiate(Resources.Load("Predator"));
            referenceTilePrey = (GameObject)Instantiate(Resources.Load("Prey"));
        }
        //tileScale = (float)5 / rows;
        //tileSize = tileScale;
        Debug.Log("tile SCale is: " + tileScale + "tile Size is: " + tileSize);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                
                Type t;
                if (entityGridD[row, col] != null)
                {
                    t = entityGridD[row, col].GetType();
                    Debug.Log($"t equals {t} at row: {row} col: {col}");
                }
                else
                {
                    t = null;
                }
                if (t == null)
                {
                    //Debug.Log("t equals null");
                }
                else if (t.Equals(typeof(Prey)))
                {
                    GameObject tilePrey = (GameObject)Instantiate(referenceTilePrey, transform);

                    float posX = col * tileSize;
                    float posY = row * -tileSize;
                    Debug.Log($"tileSize at Prey: {tileSize}, posX: {posX}, posY: {posY}");

                    Debug.Log($"Prey: row: {row} col: {col}");

                    tilePrey.transform.localScale = new Vector2(tileScale, tileScale);
                    tilePrey.transform.localPosition = new Vector2(posX, posY);
                    Prey pe = (Prey)entityGridD[row, col];
                    tilePrey.name = $"Prey({pe.getName()}) {posX} {posY}";
                    PreyRenderer = tilePrey.GetComponent<SpriteRenderer>();
                    PreyRenderer.sortingOrder = 1;
                    gameObjectGrid[row, col] = tilePrey;
                    //tilePrey.transform.parent = transform;
                }
                else if (t.Equals(typeof(Predator)))
                {
                    GameObject tilePredator = (GameObject)Instantiate(referenceTilePredator, transform);

                    float posX = col * tileSize;
                    float posY = row * -tileSize;
                    Debug.Log($"tileSize at Predator: {tileSize}, posX: {posX}, posY: {posY}");

                    Debug.Log($"Predator: row: {row} col: {col}");

                    tilePredator.transform.localScale = new Vector2(tileScale, tileScale);
                    tilePredator.transform.localPosition = new Vector2(posX, posY);
                    Predator pe = (Predator)entityGridD[row, col];
                    tilePredator.name = $"Predator({pe.getName()}) {posX} {posY}";
                    PredatorRenderer = tilePredator.GetComponent<SpriteRenderer>();
                    PredatorRenderer.sortingOrder = 1;
                    gameObjectGrid[row, col] = tilePredator;
                    //tilePredator.transform.parent = transform;
                }
            }
        }
        Destroy(referenceTilePredator);
        Destroy(referenceTilePrey);

        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
    }

    public static List<string> names = new List<string>();
    private void GenerateEntities()
    {
        int preyCount = 0;
        int predatorCount = 0;

        string path = "Assets/Resources/names.txt";
        //Read the text from directly from the names.txt file
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            names.Add(reader.ReadLine());
        }
        reader.Close();

        do
        {
            int random = UnityEngine.Random.Range(1, 3);
            int randomX = UnityEngine.Random.Range(0, rows);
            int randomY = UnityEngine.Random.Range(0, cols);
            Debug.Log($"randomX: {randomX}, randomY {randomY}");
            
            if (grid[randomX, randomY] == "G")
            {
                if (random == 1) //prey  float energyLevel, float foodLevel, float waterLevel, int maxOffsprings, float reproductionProb, int numOffsprings, float minReproductionEnergy, string name, int x, int y
                {
                    float energyLevel = UnityEngine.Random.Range(5f, 10f);
                    float foodLevel = UnityEngine.Random.Range(5f, 10f);
                    float waterLevel = UnityEngine.Random.Range(5f, 10f);
                    int maxOffsprings = UnityEngine.Random.Range(0, 6);
                    int reproductionProb = UnityEngine.Random.Range(0, 100);
                    int numOffsprings = 0;
                    float minReproductionEnergy = UnityEngine.Random.Range(2f, 10f);
                    string name = names[UnityEngine.Random.Range(0, names.Count + 1)];
                    names.Remove(name);
                    int x = randomX;
                    int y = randomY;
                    entityGrid[x, y] = new Prey(energyLevel, foodLevel, waterLevel, maxOffsprings, reproductionProb, numOffsprings, minReproductionEnergy, name, x, y);
                    Debug.Log($"Prey created at {randomX}, {randomY}. Name: {name}");
                    preyCount++;
                }
                else if (random == 2)
                {
                    float energyLevel = UnityEngine.Random.Range(5f, 10f);
                    float foodLevel = UnityEngine.Random.Range(5f, 10f);
                    float waterLevel = UnityEngine.Random.Range(5f, 10f);
                    int maxOffsprings = UnityEngine.Random.Range(0, 6);
                    int reproductionProb = UnityEngine.Random.Range(0, 100);
                    int numOffsprings = 0;
                    float minReproductionEnergy = UnityEngine.Random.Range(2f, 10f);
                    string name = names[UnityEngine.Random.Range(0, names.Count + 1)];
                    names.Remove(name);
                    int x = randomX;
                    int y = randomY;
                    entityGrid[x, y] = new Predator(energyLevel, foodLevel, waterLevel, maxOffsprings, reproductionProb, numOffsprings, minReproductionEnergy, name, x, y);
                    Debug.Log($"Predator created at {randomX}, {randomY}. Name: {name}");
                    predatorCount++;

                }
                else
                {
                    Debug.Log($"Tried to create entity at {randomX}, {randomY}. random: {random}");
                }
            }
            else
            {
                Debug.Log($"No entity created at {randomX}, {randomY} due to tile not being grass, tile was {grid[randomX, randomY]}");
            }
        } while (preyCount < minPreyCount || predatorCount < minPredatorCount || maxPreyCount > preyCount || maxPredatorCount > predatorCount);

        

        displayEntities(entityGrid);

    }
}
