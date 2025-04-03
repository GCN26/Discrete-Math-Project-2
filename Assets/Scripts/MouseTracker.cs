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
    LineRenderer currentLine;

    public List<LineDirection> lines,linesNeutral;

    public enum EdgeMode
    {
        neutralEdge,
        inOutEdge,
    }

    public EdgeMode edgeMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
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
        if (collision.GetComponent<VerticesScript>() == vertex)
        {
            vertex.hover = false;
            vertex = null;

            over = false;
        }
    }

    private void OnMouseDown()
    {
        if (over)
        {
            switch (edgeMode) {
                case EdgeMode.inOutEdge:
                    if (vert1 == null) {
                        vert1 = vertex;
                        vert1.selected = true;
                    }
                    else if (vert2 == null)
                    {
                        vert2 = vertex;

                        if (vert1 != vert2)
                        {
                            for(int i = 0; i < lines.Count; i++)
                            {
                                if (lines[i].vert1 == vert1 && lines[i].vert2 == vert2)
                                {
                                    currentLine = lines[i].GetComponent<LineRenderer>();
                                    lines[i].linesBetween += 1;
                                    break;
                                }
                            }
                            if(currentLine == null)
                            {
                                currentLine = Instantiate(line);

                                currentLine.GetComponent<LineDirection>().vert1 = vert1;
                                currentLine.GetComponent<LineDirection>().vert2 = vert2;

                                lines.Add(currentLine.GetComponent<LineDirection>());
                            }

                            //Sets lines to In Position for Vertex 2, and Out Position for Vertex 1
                            currentLine.SetPosition(0, new Vector3(vert1.outPos.transform.position.x,vert1.outPos.transform.position.y,0));
                            currentLine.SetPosition(1, new Vector3(vert2.inPos.transform.position.x, vert2.inPos.transform.position.y, 0));

                            vert1.selected = false;
                            vert1.calcLines(vert2, 1);
                            vert2.calcLines(vert1, 0);

                            currentLine = null;

                        }
                        else
                        {
                            //add loops
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
                            currentLine.SetPosition(0, new Vector3(vert1.neutralPos.transform.position.x, vert1.neutralPos.transform.position.y, 0));
                            currentLine.SetPosition(1, new Vector3(vert2.neutralPos.transform.position.x, vert2.neutralPos.transform.position.y, 0));

                            vert1.selected = false;
                            vert1.calcLines(vert2, 2);
                            vert2.calcLines(vert1, 2);

                            currentLine = null;

                        }
                        else
                        {
                            //add loops
                        }

                        vert1 = null; vert2 = null;
                    }
                    break;
            }
        }
    }
}
