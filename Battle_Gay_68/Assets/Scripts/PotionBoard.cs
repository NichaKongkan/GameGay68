using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PotionBoard : MonoBehaviour
{

    //define the size of the board
    public int width = 6;
    public int height = 8;

    //define some spacing for board
    public float spacingX;
    public float spacingY;

    //get a reference to our potion perfabs
    public GameObject[] potionPrefabs;

    //get a reference to the collection nodes potionBoard + Go
    private Node[,] potionBoard;
    public GameObject potionBoardGO;

    //layoutArray
    //public ArrayLayout arrayLayout;

    //public static of potionboard
    public static PotionBoard Instance;


    private void Awake() {
        Instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard() {
        potionBoard = new Node[width, height];

        spacingX = (float)(width - 1) / 2;
        spacingY = (float)(height - 1) / 2;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {

                Vector2 position = new Vector2(x - spacingX, y - spacingY);
                Debug.Log($"Creating potion at position: {position}"); // Debug log

                int randomIndex = Random.Range(0, potionPrefabs.Length);

                GameObject potion = Instantiate(potionPrefabs[randomIndex], position, Quaternion.identity);

                Potion potionScript = potion.GetComponent<Potion>();
                potionScript.SetIndicies(x, y);

                Node node = potion.AddComponent<Node>();
                node.Initialize(true, potion);

            
                potionBoard[x, y] = node;
            }
        }
    }
}
