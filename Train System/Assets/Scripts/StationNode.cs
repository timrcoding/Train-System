using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationNode : MonoBehaviour
{
    [SerializeField]
    private GameObject trainLine;

    public bool startingNode;

    public List<string> trainlines;

    void Start()
    {
        
    }

    public void setImage()
    {
        if(trainlines.Count > 1)
        {
            GetComponent<Image>().sprite = GameManager.instance.junctionSprite;
            GetComponent<Image>().color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.clear;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Node")
        {
            if(other.GetInstanceID() > GetInstanceID())
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void setUpTrainline()
    {
        startingNode = true;
        GameObject newLine = Instantiate(trainLine, transform.position, Quaternion.identity);
        GameObject trainLineParent = GameObject.Find("TrainlineParent");
        newLine.transform.SetParent(trainLineParent.transform);
        newLine.transform.localScale = new Vector3(1, 1, 1);
        newLine.GetComponent<TrainLine>().refNumber = GameManager.instance.lineCount;
        GameManager.instance.lineCount++;
    }

    
}
