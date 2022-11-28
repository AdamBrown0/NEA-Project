using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GridManager : MonoBehaviour
{

    public static int rows;
    public static int cols;
    public static int minPreyCount;
    public static int minPredatorCount;
    public static int maxPreyCount;
    public static int maxPredatorCount;


    private string[,] grid = new string[rows, cols];
    private object[,] entityGrid = new object[rows, cols];


    private float tileSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //rows = first.rows;
        //cols = first.cols;
        Debug.ClearDeveloperConsole();
        GenerateGrid();
        GenerateEntities();
    }

    private void GenerateGrid()
    {
        //need to make minimum entity count because sometimes none spawn.

        //setting rules for generating non-grass tiles
        int barrierCount = 0;
        int maxBarrier = (rows * cols) / 5;
        int waterCount = 0;
        int maxWater = (rows * cols) / 5;
        int dotCount = 0;

        int maxDot = (rows * cols) / 10;

        //makes sure square
        //cols = 8 * (5 / rows);

        //makes scale nice
        float tileScale = (float) 5 / rows;
        tileSize = tileScale;


        //reference Tiles for ease of access later on
        GameObject referenceTileWater = (GameObject)Instantiate(Resources.Load("Water"));
        GameObject referenceTileGrass = (GameObject)Instantiate(Resources.Load("Grass"));
        GameObject referenceTileBarrier = (GameObject)Instantiate(Resources.Load("Barrier"));
       

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

    private void GenerateEntities()
    {
        int preyCount = 0;
        int predatorCount = 0;
        List<string> names = new List<string>();

        string path = "Assets/Resources/names.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            names.Add(reader.ReadLine());
        }
        reader.Close();

        int randx = 0;
        int randy = 0;

        do
        {
            int random = UnityEngine.Random.Range(1, 3);
            int randomX = UnityEngine.Random.Range(0, rows);
            int randomY = UnityEngine.Random.Range(0, cols);
            Debug.Log($"randomX: {randomX}, randomY {randomY}");
            randx = randomX;
            randy = randomY;
            if (grid[randomX, randomY] == "G")
            {
                if (random == 1) //prey  float energyLevel, float foodLevel, float waterLevel, int maxOffsprings, float reproductionProb, int numOffsprings, float minReproductionEnergy, string name, int x, int y
                {
                    float energyLevel = UnityEngine.Random.Range(5f, 10f);
                    float foodLevel = UnityEngine.Random.Range(5f, 10f);
                    float waterLevel = UnityEngine.Random.Range(5f, 10f);
                    int maxOffsprings = UnityEngine.Random.Range(0, 6);
                    float reproductionProb = UnityEngine.Random.Range(0f, 100f);
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
                    float reproductionProb = UnityEngine.Random.Range(0f, 100f);
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

        Type t = entityGrid[randx, randy].GetType();
        Debug.Log($"randx: {randx}, randy {randy}");
        if (t.Equals(typeof(Prey)))
        {
            Prey p = (Prey) entityGrid[randx, randy];
            Debug.Log("name: " + p.getName());
            Debug.Log("water: " + p.getWaterLevel());
            string[,] blockNeighbours = p.getBlockNeighbours(grid);
            for (int rows = 0; rows < blockNeighbours.GetLength(0); rows++)
            {
                for (int cols = 0; cols < blockNeighbours.GetLength(1); cols++)
                {
                    if (blockNeighbours[rows, cols] != null)
                    {
                        Debug.Log($"Tile {blockNeighbours[rows, cols]} at {rows} {cols}");
                    }
                    else
                    {
                        Debug.Log($"getBlockNeighbours returned null");
                    }
                }
            }
            object[,] entityNeighbours = p.getEntityNeighbours(entityGrid);
            for (int rows = 0; rows < entityNeighbours.GetLength(0); rows++)
            {
                for (int cols = 0; cols < entityNeighbours.GetLength(1); cols++)
                {
                    if (entityNeighbours[rows, cols] != null)
                    {
                        Type tp = entityNeighbours[rows, cols].GetType();
                        Prey pe = (Prey)entityNeighbours[rows, cols];
                        Debug.Log($"Tile {tp.ToString()} at {rows} {cols}. name: {pe.getName()}");
                    }
                    else
                    {
                        Debug.Log($"getBlockNeighbours returned null");
                    }
                }
            }
        }
        else if (t.Equals(typeof(Predator)))
        {
            Predator pred = (Predator)entityGrid[randx, randy];
            Debug.Log("name: " + pred.getName());
            Debug.Log("water: " + pred.getWaterLevel());
            string[,] blockNeighbours = pred.getBlockNeighbours(grid);
            for (int rows = 0; rows < blockNeighbours.GetLength(0); rows++)
            {
                for (int cols = 0; cols < blockNeighbours.GetLength(1); cols++)
                {
                    if (blockNeighbours[rows, cols] != null)
                    {
                        Debug.Log($"Tile {blockNeighbours[rows, cols]} at {rows} {cols}");
                    }
                    else
                    {
                        Debug.Log($"getBlockNeighbours returned null");
                    }
                    
                }
            }
            object[,] entityNeighbours = pred.getEntityNeighbours(entityGrid);
            for (int rows = 0; rows < entityNeighbours.GetLength(0); rows++)
            {
                for (int cols = 0; cols < entityNeighbours.GetLength(1); cols++)
                {
                    if (entityNeighbours[rows, cols] != null)
                    {
                        Type tp = entityNeighbours[rows, cols].GetType();
                        Predator pe = (Predator)entityNeighbours[rows, cols];
                        Debug.Log($"Tile {tp.ToString()} at {rows} {cols}. name: {pe.getName()}");
                    }
                    else
                    {
                        Debug.Log($"getBlockNeighbours returned null");
                    }
                    
                }
            }
        }

        GameObject referenceTilePredator = (GameObject)Instantiate(Resources.Load("Predator"));
        GameObject referenceTilePrey = (GameObject)Instantiate(Resources.Load("Prey"));
        float tileScale = (float)5 / rows;
        

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (entityGrid[row, col] != null)
                {
                    t = entityGrid[row, col].GetType();
                }
                else
                {
                    t = null;
                }
                if (t == null)
                {
                    Debug.Log("t equals null");
                }
                else if (t.Equals(typeof(Prey)))
                {
                    GameObject tilePrey = (GameObject)Instantiate(referenceTilePrey, transform);

                    float posX = col * tileSize;
                    float posY = row * -tileSize;

                    tilePrey.transform.localScale = new Vector2(tileScale, tileScale);
                    tilePrey.transform.position = new Vector2(posX, posY);
                    Prey pe = (Prey)entityGrid[row, col];
                    tilePrey.name = $"Prey({pe.getName()}) {posX} {posY}";
                }
                else if (t.Equals(typeof(Predator)))
                {
                    GameObject tilePredator = (GameObject)Instantiate(referenceTilePredator, transform);

                    float posX = col * tileSize;
                    float posY = row * -tileSize;

                    tilePredator.transform.localScale = new Vector2(tileScale, tileScale);
                    tilePredator.transform.position = new Vector2(posX, posY);
                    Predator pe = (Predator)entityGrid[row, col];
                    tilePredator.name = $"Predator({pe.getName()}) {posX} {posY}";
                }
            }
        }
        Destroy(referenceTilePredator);
        Destroy(referenceTilePrey);

    }

    //private object[,] createEntity(object[,] entityGridC, Prey entityPrey, Predator entityPredator, int posX, int posY)
    //{

    //    entityGridC[posX, posY] = entityPrey;

    //    Debug.Log($"ENTITY AT {posX} {posY}, name {entityPrey.getName()} type: {entityGridC[posX, posY]}");

    //    return entityGridC;
    //}

    IEnumerator moveDot()
    {
        float tileScale = (float)5 / rows;

        GameObject referenceTileDot = (GameObject)Instantiate(Resources.Load("Dot"));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int randomMove = UnityEngine.Random.Range(1, 5);

                if (grid[row, col] == "D")
                {
                    if (randomMove == 1)
                    {
                        try
                        {
                            grid[row - 1, col] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else if (randomMove == 2)
                    {
                        try
                        {
                            grid[row, col - 1] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else if (randomMove == 3)
                    {
                        try
                        {
                            grid[row + 1, col] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else if (randomMove == 4)
                    {
                        try
                        {
                            grid[row, col + 1] = "Dot";
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Exception thrown {e}");
                            Debug.Log($"Row = {row}, Col = {col}");
                            Debug.Log("Dot reached edge, skipping move");
                        }
                    }
                    else
                    {
                        Debug.Log($"Error, randomMove was {randomMove}");
                    }

                    GameObject tileDot = (GameObject)Instantiate(referenceTileDot, transform);

                    float posXX = col * tileSize;
                    float posYY = row * -tileSize;

                    tileDot.transform.localScale = new Vector2(tileScale, tileScale);
                    tileDot.transform.position = new Vector2(posXX, posYY);
                    tileDot.name = $"Dot {posXX} {posYY}";
                    grid[row, col] = "D";

                }
            }
        }

        //wait for 2 seconds
        yield return new WaitForSeconds(2);
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(moveDot());
    }
}
