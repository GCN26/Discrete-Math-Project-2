using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static MouseTracker;

public class VerticesScript : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public SpriteRenderer spriteRenderer;

    public bool selected;
    public bool hover;
    //Vertex and Int lists share an index
    public List<VerticesScript> edgesIn, edgesOut,edgesNeutral;
    public List<int> edgesInCount, edgesOutCount, edgesNeutralCount;

    public List<LineRenderer> connectedNLines,connectedInLines, connectedOutLines;
    public List<int> indexesN;

    public GameObject inPos, outPos, neutralPos;

    public VertexManager manager;
    public TextMeshProUGUI number;

    public MouseTracker mouse;

    public GameObject loop, dirLoop;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(mouse == null)
        {
            mouse = GameObject.Find("Rat").GetComponent<MouseTracker>();
        }
        if (manager == null)
        {
            manager = GameObject.Find("VertexManager").GetComponent<VertexManager>();
            manager.vertexList.Add(this);
            manager.updateLists(this);
        }
            for (int i = 0; i < manager.vertexList.Count; i++)
        {
            edgesIn.Add(manager.vertexList[i]);
            edgesOut.Add(manager.vertexList[i]);
            edgesNeutral.Add(manager.vertexList[i]);
            edgesInCount.Add(0);
            edgesOutCount.Add(0);
            edgesNeutralCount.Add(0);
            if (manager.vertexList[i] == this)
            {
                number.text = (i+1).ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected && !hover)
        {
            spriteRenderer.color = Color.red;
        }
        else if(!selected && hover)
        {
            spriteRenderer.color = Color.blue;
        }
        else if(!selected && !hover)
        {
            spriteRenderer.color = Color.white;
        }

        for(int i = 0;i<connectedNLines.Count;i++)
        {
            connectedNLines[i].SetPosition(indexesN[i],neutralPos.transform.position);
        }
        for (int i = 0; i < connectedInLines.Count; i++)
        {
            connectedInLines[i].SetPosition(1, inPos.transform.position);
        }
        for (int i = 0; i < connectedOutLines.Count; i++)
        {
            connectedOutLines[i].SetPosition(0, outPos.transform.position);
        }
        if(loop != null)
        {
            loop.transform.position = new Vector3(this.transform.position.x + .5f, this.transform.position.y + .5f, -1);
        }
        if (dirLoop != null)
        {
            dirLoop.transform.position = new Vector3(this.transform.position.x - .5f, this.transform.position.y + .5f, -1);
        }
    }

    public void addVertexToLists(VerticesScript vert1)
    {
        edgesIn.Add(vert1);
        edgesOut.Add(vert1);
        edgesNeutral.Add(vert1);
        edgesInCount.Add(0);
        edgesOutCount.Add(0);
        edgesNeutralCount.Add(0);
    }

    private void OnMouseOver()
    {
    }
    /// <summary>
    /// InOutNeutral: 0 = In, 1 = Out, 2 = Neutral
    /// </summary>
    /// <param name="altVert"></param>
    /// <param name="InOutNeutral"></param>
    public void calcLines(VerticesScript altVert,int InOutNeutral)
    {
        switch (InOutNeutral)
        {
            case 0:
                if (edgesIn.Count > 0)
                {
                    for (int i = 0; i < edgesIn.Count; i++)
                    {
                        if (altVert == edgesIn[i])
                        {
                            edgesInCount[i] += 1;
                            break;
                        }
                        else if (altVert != edgesIn[i] && i == edgesIn.Count - 1)
                        {
                            edgesIn.Add(altVert);
                            edgesInCount.Add(0);
                        }
                    }
                }
                else
                {
                    edgesIn.Add(altVert);
                    edgesInCount.Add(1);
                }
                break;
            case 1:
                if (edgesOut.Count > 0)
                {
                    for (int i = 0; i < edgesOut.Count; i++)
                    {
                        if (altVert == edgesOut[i])
                        {
                            edgesOutCount[i] += 1;
                            break;
                        }
                        else if (altVert != edgesOut[i] && i == edgesOut.Count - 1)
                        {
                            edgesOut.Add(altVert);
                            edgesOutCount.Add(0);
                        }
                    }
                }
                else
                {
                    edgesOut.Add(altVert);
                    edgesOutCount.Add(1);
                }
                break;
            case 2:
                if (edgesNeutral.Count > 0)
                {
                    for (int i = 0; i < edgesNeutral.Count; i++)
                    {
                        if (altVert == edgesNeutral[i])
                        {
                            edgesNeutralCount[i] += 1;
                            break;
                        }
                        else if (altVert != edgesNeutral[i] && i == edgesNeutral.Count - 1)
                        {
                            edgesNeutral.Add(altVert);
                            edgesNeutralCount.Add(0);
                        }
                    }
                }
                else
                {
                    edgesNeutral.Add(altVert);
                    edgesNeutralCount.Add(1);
                }
                break;
        }
    }
    public void OnMouseDown()
    {
        if (mouse.mouseContact == mouseMode.drag)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }
    public void OnMouseDrag()
    {
        if (mouse.mouseContact == mouseMode.drag)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }
}
