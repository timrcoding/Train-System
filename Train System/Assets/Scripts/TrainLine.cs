using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TrainLine : MonoBehaviour
{
    [SerializeField]
    private GameObject endPoint;
    [SerializeField]
    private List<GameObject> pointsOnRoute;
    [SerializeField]
    private List<GameObject> allOtherNodes;
    [SerializeField]
    private GameObject railLine;

    [SerializeField]
    private Color routeColor;
    [SerializeField]
    private string routeName;
    public int refNumber;
    void Start()
    {
        routeName = "Route" + GetInstanceID().ToString();
        routeColor = Color.HSVToRGB(Random.Range(0.3f, 1), 1, 1);
        gatherAllNodes();
        
        
    }

    void gatherAllNodes()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        //ADD TO PLACED LIST
        foreach (GameObject node in nodes)
        {
            if (node != gameObject)
            {
                allOtherNodes.Add(node);
            }
        }
        sortNodesNearestToFurthest(transform.position);
        pickFinishPoint(allOtherNodes.Count - 1);
        pickPoints();
        createRailLines();
    }

    void sortNodesNearestToFurthest(Vector2 pos)
    {
        allOtherNodes = allOtherNodes.OrderBy(
        x => Vector2.Distance(pos, x.transform.position)).ToList();
    }

    void pickFinishPoint(int count)
    {
        StationNode sNode = allOtherNodes[count].GetComponent<StationNode>();
        if (!sNode.startingNode)
        {
            endPoint = sNode.gameObject;
        }
        else
        {
            pickFinishPoint(count - 1);
        }
    }

    void pickPoints()
    {
        pointsOnRoute.Add(allOtherNodes[0]);

        int stops = 5;
        for(int i = 0; i < stops; i++)
        {
            float mappedValue = (map(i, 0, stops, 0, 1));
            Vector2 point = Vector2.Lerp(transform.position, endPoint.transform.position, mappedValue);
            sortNodesNearestToFurthest(point);
            pointsOnRoute.Add(allOtherNodes[0]);
        }

        pointsOnRoute.Add(endPoint);
    }

    void createRailLines()
    {
        for(int i = 0; i < pointsOnRoute.Count-1; i++)
        {
            drawLine(i);
            addStationInformation(i);
        }
    }

    void addStationInformation(int i)
    {
        StationNode sNode = pointsOnRoute[i].GetComponent<StationNode>();
        sNode.trainlines.Add(routeName);
        sNode.setImage();
    }

    void drawLine(int i)
    {
        GameObject line = Instantiate(railLine, transform.position, Quaternion.identity);
        LineRenderer lineR = line.GetComponent<LineRenderer>();
        lineR.SetPosition(0, new Vector2(pointsOnRoute[i].transform.position.x, pointsOnRoute[i].transform.position.y));
        lineR.SetPosition(1, new Vector2(pointsOnRoute[i + 1].transform.position.x, pointsOnRoute[i + 1].transform.position.y));
        lineR.startColor = GameManager.instance.lineColors[refNumber];
        lineR.endColor = GameManager.instance.lineColors[refNumber];
        lineR.transform.SetParent(transform);
        lineR.transform.localScale = new Vector3(1, 1, 1);
    }

    

    public float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    void setStartPoint()
    {
        pointsOnRoute.Insert(0, gameObject);
    }

    void colourPoints()
    {
        for(int i = 1; i < pointsOnRoute.Count; i++)
        {
           pointsOnRoute[i].GetComponent<Image>().color = routeColor;
        }
    }

    
}
