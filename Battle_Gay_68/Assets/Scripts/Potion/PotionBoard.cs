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
    public ArrayLayout arrayLayout;

    //public static of potionBoard
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
                if (arrayLayout.rows[y].row[x]) {
                    potionBoard[x, y] = new Node(false, null);
                } else {
                    int randomIndex = Random.Range(0, potionPrefabs.Length);

                    GameObject potion = Instantiate(potionPrefabs[randomIndex], position, Quaternion.identity);

                    potion.GetComponent<Potion>().SetIndicies(x, y);
                    potionBoard[x, y] = new Node(true, potion);
                }
                
            }
        }
        if (CheckBoard()) {
            Debug.Log("We have matches let's re-create the baord");
            InitializeBoard();
        } else {
            Debug.Log("There are no matches, it's time to start the game!");
        }
    }

    public bool CheckBoard()
    {
        Debug.Log("Checking Board");
        bool hasMatched = false;

        List<Potion> potionToRemove = new();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                //checking if potion node is usable
                if (potionBoard[x, y].isUsable) {
                    //then proceed to get potion class in node.
                    Potion potion = potionBoard[x, y].potion.GetComponent<Potion>();

                    //ensure its not matched
                    if(!potion.isMatched) {
                        //run some matching logic

                        MatchResult matchedPotions = IsConnected(potion); 

                        if (matchedPotions.connectedPotions.Count >= 3) {
                            //coplex matching...

                            potionToRemove.AddRange(matchedPotions.connectedPotions);

                            foreach (Potion pot in matchedPotions.connectedPotions)
                                pot.isMatched = true;

                            hasMatched = true;
                        }
                    }
                }
            }
        }
        return hasMatched;
    }

    MatchResult IsConnected(Potion potion) {
        List<Potion> connectedPotions = new();
        Potion.PotionType potionType = potion.potionType;

        connectedPotions.Add(potion);

        //check right
        CheckDirection(potion, new Vector2Int(1, 0), connectedPotions);

        //check left
        CheckDirection(potion, new Vector2Int(-1, 0), connectedPotions);

        //have we made a 3 match? (Horizontal Match)
        if (connectedPotions.Count == 3) {
            Debug.Log("I have a normal Horizontal match, the color of my match is: " + connectedPotions[0].potionType);

            return new MatchResult {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Horizontal
            };
        }

        //check for more then 3 (Long horizontal Match)
        else if (connectedPotions.Count > 3) {
            Debug.Log("I have a Long Horizontal match, the color of my match is: " + connectedPotions[0].potionType);

            return new MatchResult {
                connectedPotions = connectedPotions,
                direction = MatchDirection.LongHorizontal
            };
        }

        //clear out connectedpotions
        connectedPotions.Clear();
        //readd our initial potion
        connectedPotions.Add(potion);

        //check up
        CheckDirection(potion, new Vector2Int(0, 1), connectedPotions);
        //check dowe
        CheckDirection(potion, new Vector2Int(0, -1), connectedPotions);

        //have we made a 3 match? (Vertcal Match)
        if (connectedPotions.Count == 3) {
            Debug.Log("I have a normal vertical match, the color of my match is: " + connectedPotions[0].potionType);

            return new MatchResult {
                connectedPotions = connectedPotions,
                direction = MatchDirection.Verical
            };
        }

        //check for more then 3 (Long horizontal Match)
        else if (connectedPotions.Count > 3) {
            Debug.Log("I have a Long vertical match, the color of my match is: " + connectedPotions[0].potionType);

            return new MatchResult {
                connectedPotions = connectedPotions,
                direction = MatchDirection.LongVertical
            };
        } else {
            return new MatchResult {
                connectedPotions = connectedPotions,
                direction = MatchDirection.None
            };
        }
    }

    void CheckDirection(Potion pot, Vector2Int direction, List<Potion> connectedPotions) {
        Potion.PotionType potionType = pot.potionType;
        int x = pot.xIndex + direction.x;
        int y = pot.yIndex + direction.y;

        //check that we're within the bounderies of the board
        while (x >= 0 && x < width && y >= 0 && y < height) {
            if (potionBoard[x,y].isUsable) {
                Potion neighourPotion = potionBoard[x, y].potion.GetComponent<Potion>();

                if(!neighourPotion.isMatched && neighourPotion.potionType == potionType) {
                    connectedPotions.Add(neighourPotion);

                    x += direction.x;
                    y += direction.y;
                } else {
                    break;
                }
            } else {
                break;
            }
        }
    }
}

public class MatchResult {
    public List<Potion> connectedPotions;
    public MatchDirection direction;
}

public enum MatchDirection {
    Verical,
    Horizontal,
    LongVertical,
    LongHorizontal,
    Super,
    None
}