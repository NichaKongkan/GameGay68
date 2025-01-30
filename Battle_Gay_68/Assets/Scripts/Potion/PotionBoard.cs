using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public List<GameObject> potionsToDestroy = new();

    [SerializeField]
    private Potion selectedPotion;

    [SerializeField]
    private bool isProcessingMove;

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

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Potion>()) {
                if (isProcessingMove)
                    return;

                Potion potion = hit.collider.gameObject.GetComponent<Potion>();
                Debug.Log("I have a clicked a potion it is: " + potion.gameObject);
                SelectPotion(potion);
            }
        }
    }

    void InitializeBoard() {
        DestroyPotions();
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
                    potionsToDestroy.Add(potion);
                }
                
            }
        }
        if (CheckBoard()) {
            Debug.Log("We have matches let's re-create the board");
            InitializeBoard();
        } else {
            Debug.Log("There are no matches, it's time to start the game!");
        }
    }

    private void DestroyPotions()
    {
        if (potionsToDestroy != null)
        {
            foreach (GameObject potion in potionsToDestroy)
            {
                Destroy(potion);
            }
            potionsToDestroy.Clear();
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

    #region Swappin Potions

    //Select potion
    public void SelectPotion(Potion _potion) {
        // if we don't have a potion currently selected, then set the potion i just clicked to my selectedpotion
        if (selectedPotion == null) {
            Debug.Log(_potion);
            selectedPotion = _potion;
        }
        // if we select the same potion twice, then let's make selectpotion null
        else if (selectedPotion == _potion) {
            selectedPotion = null;
        }
        // if selectedpotion is not and is not the current potion, attempt a swap
        //selectedpotion back to null
        else if (selectedPotion != _potion){
            SwapPotion(selectedPotion, _potion);
            selectedPotion = null;
        }
    }

    //swap potion - logic
    private void SwapPotion(Potion _currentPotion, Potion _targetPotion){
        if (!IsAdjacent(_currentPotion, _targetPotion)) {
            return;
        }

        DoSwap(_currentPotion, _targetPotion);

        isProcessingMove = true;

        StartCoroutine(ProcessMatches(_currentPotion, _targetPotion));
    }
    //do swap
    private void DoSwap(Potion _currentPotion, Potion _targetPotion) {
        GameObject temp = potionBoard[_currentPotion.xIndex, _currentPotion.yIndex].potion;

        potionBoard[_currentPotion.xIndex, _currentPotion.yIndex].potion = potionBoard[_targetPotion.xIndex, _targetPotion.yIndex].potion;
        potionBoard[_targetPotion.xIndex, _targetPotion.yIndex].potion = temp;

        //update indicies.
        int tempXIndex = _currentPotion.xIndex;
        int tempYIndex = _currentPotion.yIndex;
        _currentPotion.xIndex = _targetPotion.xIndex;
        _currentPotion.yIndex = _targetPotion.yIndex;
        _targetPotion.xIndex = tempXIndex;
        _targetPotion.yIndex = tempYIndex;

        _currentPotion.MoveToTarget(potionBoard[_targetPotion.xIndex, _targetPotion.yIndex].potion.transform.position);

        _targetPotion.MoveToTarget(potionBoard[_currentPotion.xIndex, _currentPotion.yIndex].potion.transform.position);
    }

    private IEnumerator ProcessMatches(Potion _currentPotion, Potion _targetPotion) {
        yield return new WaitForSeconds(0.2f);
        bool hasMatch = CheckBoard();
        if (!hasMatch) {
            DoSwap(_currentPotion, _targetPotion);
        }
        isProcessingMove = false;
    }

    //IsAdjacent
    private bool IsAdjacent(Potion _currentPotion, Potion _targetPotion) {
        return Mathf.Abs(_currentPotion.xIndex - _targetPotion.xIndex) + Mathf.Abs(_currentPotion.yIndex - _targetPotion.yIndex) == 1;
    }

    //ProcessMatches
    




    #endregion
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