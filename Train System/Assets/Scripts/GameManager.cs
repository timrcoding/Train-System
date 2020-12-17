using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int numberOfLines;
    public int lineCount;

    [SerializeField]
    private GameObject nodeParent;


    [SerializeField]
    int stationNodeCount;
    [SerializeField]
    private GameObject stationNode;

    public List<GameObject> placedNodes;

    public Color[] lineColors;

    public Sprite junctionSprite;
    

    void Start()
    {
        instance = this;
        spawnStationNodes();
        StartCoroutine(addToList());
    }

    //SPAWN IN POSSIBLE STATION POSITION
    void spawnStationNodes()
    {
        for(int i = 0; i < stationNodeCount; i++)
        {
            Vector2 placement = Random.insideUnitCircle * 4.5f;
            GameObject newNode = Instantiate(stationNode, placement, Quaternion.identity);
            newNode.transform.SetParent(nodeParent.transform);
            newNode.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    //ADD TO A LIST AND SORT BY DISTANCE FROM CENTRE;
    IEnumerator addToList()
    {
        yield return new WaitForEndOfFrame();
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        //ADD TO PLACED LIST
        foreach (GameObject node in nodes)
        {
            placedNodes.Add(node);
        }
        sortNodesByDistance();
        pickStartingStations();
    }

    void sortNodesByDistance()
    {
        placedNodes = placedNodes.OrderBy(
        x => Vector2.Distance(transform.position, x.transform.position)).ToList();
    }

    void pickStartingStations()
    {
         List<GameObject> possibleStations = new List<GameObject>();
        for(int i = placedNodes.Count -10; i < placedNodes.Count; i++)
        {
            possibleStations.Add(placedNodes[i]);            
        }
        for(int j = 0; j < numberOfLines; j++)
        {
            int randNum = Random.Range(0, possibleStations.Count-1);
            possibleStations[randNum].GetComponent<StationNode>().setUpTrainline();
            possibleStations.RemoveAt(randNum);
        }
    }

    
}
