using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseTracker : MonoBehaviour
{
    VerticesScript vertex;
    public bool over;

    [SerializeField]
    VerticesScript vert1, vert2;

    public LineRenderer line, lineNeutral;
    public GameObject loop,directionLoop;

    LineRenderer currentLine;

    public List<LineDirection> lines,linesNeutral;

    public enum EdgeMode
    {
        neutralEdge,
        inOutEdge,
    }

    public enum mouseMode
    {
        drag,
        edge,
        view
    }

    public EdgeMode edgeMode;
    public mouseMode mouseContact;

    public TMP_Dropdown edgeDrop, mouseDrop;
    public TextMeshProUGUI connects;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(edgeDrop.value == 0)
        {
            edgeMode = EdgeMode.neutralEdge;
        }
        else if(edgeDrop.value == 1)
        {
            edgeMode = EdgeMode.inOutEdge;
        }

        if(mouseDrop.value == 0)
        {
            mouseContact = mouseMode.drag;
        }
        else if (mouseDrop.value == 1)
        {
            mouseContact = mouseMode.edge;
        }
        else if (mouseDrop.value == 2)
        {
            mouseContact = mouseMode.view;
        }

        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        if(mouseContact == mouseMode.edge || mouseContact == mouseMode.view)
        {
            this.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            this.GetComponent<Collider2D>().enabled = false;
        }

        if(vertex != null && mouseContact == mouseMode.view)
        {
            string conString;
            conString = "Vertex #" + vertex.numberOfVert.ToString() + "\nNeutral Connections: " + vertex.getConnections(0) + "\nIn Connections: " + vertex.getConnections(1) + "\nOut Connections: " + vertex.getConnections(2);
            connects.text = conString;
        }
        else
        {
            connects.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<VerticesScript>() != null)
        {
            vertex = collision.GetComponent<VerticesScript>();
            vertex.hover = true;
            over = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<VerticesScript>() == vertex && vertex != null)
        {
            vertex.hover = false;
            vertex = null;

            over = false;
        }
    }

    private void OnMouseDown()
    {
        if (mouseContact == mouseMode.edge)
        {
            if (over)
            {
                switch (edgeMode)
                {
                    case EdgeMode.inOutEdge:
                        if (vert1 == null)
                        {
                            vert1 = vertex;
                            vert1.selected = true;
                        }
                        else if (vert2 == null)
                        {
                            vert2 = vertex;

                            if (vert1 != vert2)
                            {
                                for (int i = 0; i < lines.Count; i++)
                                {
                                    if (lines[i].vert1 == vert1 && lines[i].vert2 == vert2)
                                    {
                                        currentLine = lines[i].GetComponent<LineRenderer>();
                                        lines[i].linesBetween += 1;
                                        break;
                                    }
                                }
                                if (currentLine == null)
                                {
                                    currentLine = Instantiate(line);

                                    currentLine.GetComponent<LineDirection>().vert1 = vert1;
                                    currentLine.GetComponent<LineDirection>().vert2 = vert2;

                                    lines.Add(currentLine.GetComponent<LineDirection>());
                                }

                                //Sets lines to In Position for Vertex 2, and Out Position for Vertex 1

                                vert1.connectedOutLines.Add(currentLine);
                                vert2.connectedInLines.Add(currentLine);

                                vert1.selected = false;
                                vert1.calcLines(vert2, 1);
                                vert2.calcLines(vert1, 0);

                                currentLine = null;

                            }
                            else
                            {
                                //add loops
                                GameObject loopA = Instantiate(directionLoop);
                                vert1.selected = false;
                                loopA.GetComponent<SpriteRenderer>().sortingOrder = -1;
                                vert1.calcLines(vert1, 0);
                                vert1.calcLines(vert1, 1);
                                vert1.dirLoop = loopA;
                            }

                            vert1 = null; vert2 = null;
                        }
                        break;
                    case EdgeMode.neutralEdge:
                        if (vert1 == null)
                        {
                            vert1 = vertex;
                            vert1.selected = true;
                        }
                        else if (vert2 == null)
                        {
                            vert2 = vertex;

                            if (vert1 != vert2)
                            {
                                for (int i = 0; i < linesNeutral.Count; i++)
                                {
                                    if (linesNeutral[i].vert1 == vert1 && linesNeutral[i].vert2 == vert2)
                                    {
                                        currentLine = linesNeutral[i].GetComponent<LineRenderer>();
                                        linesNeutral[i].linesBetween += 1;
                                        break;
                                    }
                                }
                                if (currentLine == null)
                                {
                                    currentLine = Instantiate(lineNeutral);

                                    currentLine.GetComponent<LineDirection>().vert1 = vert1;
                                    currentLine.GetComponent<LineDirection>().vert2 = vert2;

                                    linesNeutral.Add(currentLine.GetComponent<LineDirection>());
                                }

                                //Sets lines to Neutral Position in the vertex
                                vert1.connectedNLines.Add(currentLine);
                                vert1.indexesN.Add(0);
                                vert2.connectedNLines.Add(currentLine);
                                vert2.indexesN.Add(1);

                                vert1.selected = false;
                                vert1.calcLines(vert2, 2);
                                vert2.calcLines(vert1, 2);

                                currentLine = null;

                            }
                            else
                            {
                                //add loops
                                GameObject loopA = Instantiate(loop);
                                vert1.selected = false;
                                loopA.GetComponent<SpriteRenderer>().sortingOrder = -1;
                                vert1.calcLines(vert1, 2);
                                vert1.calcLines(vert1, 2);
                                vert1.loop = loopA;
                            }

                            vert1 = null; vert2 = null;
                        }
                        break;
                }
            }
            else if(vert1 != null && vert2 == null)
            {
                vert1.selected = false;
                vert1 = null;
            }
        }
        
    }

    
}
