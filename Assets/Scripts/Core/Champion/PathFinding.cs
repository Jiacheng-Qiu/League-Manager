using System.Collections;
using UnityEngine;

// A* path finding using grid of size 30*16 representing map, assign to gameobjects needed
public class PathFinding : MonoBehaviour
{
    private bool[,] mapGrid; // Walkable spaces marked as true, other space marked as false
    // The map is symmetry, the terrain pos only records the left half
    private string terrainPos = "(0,0),(0,1),(0,2),(0,3),(0,6),(0,7),(1,0),(1,1),(1,2),(1,6),(1,7),(1,8),(2,0),(2,1),(3,0),(3,8),(6,8),(6,9),(6,10),(6,11),(7,8),(7,9),(7,10),(7,11),(8,2),(8,8),(8,9),(8,10),(8,11),(8,12),(8,13),(9,2),(9,8),(9,9),(9,10),(10,10),(12,6),(12,13),(13,5),(13,10),(13,11),(13,12),(14,5),(14,7),(14,8),(14,11),(14,12),";
    // Arraylists defined for A*
    private ArrayList open;
    private ArrayList close;
    
    private void Start()
    {
        mapGrid = new bool[30, 16];
        string terrainCopy = (string)terrainPos.Clone();
        while (terrainCopy.Length >= 1)
        {
            // Find the x,y in each position, and assign correcponding in grid as false
            int start = terrainCopy.IndexOf("(");
            int end = terrainCopy.IndexOf(",");
            //Debug.Log(terrainCopy.Substring(start + 1, end - start - 1));
            int x = int.Parse(terrainCopy.Substring(start + 1, end - start - 1));
            start = terrainCopy.IndexOf(")");
            //Debug.Log(terrainCopy.Substring(end + 1, start - end - 1));
            int y = int.Parse(terrainCopy.Substring(end + 1, start - end - 1));
            // The position and symmetry position are both marked as terrain
            mapGrid[x, y] = true;
            mapGrid[29 - x, y] = true;

            // Go to next position (x,y)
            terrainCopy = terrainCopy.Substring(terrainCopy.IndexOf("),") + 2);
        }
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                // Set available positions as true, and terrain as false
                mapGrid[i, j] = !mapGrid[i, j];
                // Debug use
                /*if (mapGrid[i, j])
                {
                    Object a = Instantiate(pref, pos + new Vector3(i, j, 0), transform.rotation);
                    a.name = "Pos" + i + "," + j;
                }*/
            }
        }
        open = new ArrayList();
        close = new ArrayList();

        // Debug use
        /*Node endNode = FindPath(new Vector3(0, 3, 0), new Vector3(12,6,0));
        while (endNode.parent != null)
        {
            Debug.Log(endNode.pos);
            //GameObject.Find("Pos" + endNode.pos.x + "," + endNode.pos.y).transform.localScale = new Vector3(0.6f, 0.6f, 1);
            endNode = endNode.parent;
        }*/
    }

    // Node class for A* that records the calced FGH values
    public class Node
    {
        public int Gcost; // Cost from startin pos
        public int Hcost; // Cost to ending pos
        public int FCost; // Sum of G and H
        public Vector2 pos; // Position in grid on map
        public Node parent; // Set parent node
        public Node(int G, int H, int x, int y)
        {
            Gcost = G;
            Hcost = H;
            FCost = G + H;
            pos = new Vector2(x, y);
        }
    }

    // Find the path using A*
    public Node FindPath(Vector3 startPos, Vector3 endPos)
    {
        // Find the square pos of start and end pos
        int startX = (int)startPos.x;
        int startY = (int)startPos.y;
        int endX = (int)endPos.x;
        int endY = (int)endPos.y;
        // Check if any of the square is terrain, if so find the closest available ones
        Vector2 response = CheckAvailable(startPos);
        if (response.x != -1)
        {
            startX = (int)response.x;
            startY = (int)response.y;
        }
        response = CheckAvailable(endPos);
        if (response.x != -1)
        {
            endX = (int)response.x;
            endY = (int)response.y;
        }

        open.Add(new Node(0, Mathf.Abs(endX - startX) + Mathf.Abs(endY - startY), startX, startY));
        ((Node)open[0]).parent = null;
        Node current;
        while (true)
        {
            // current eval set as node with lowest F
            current = FindLowestInOpen();
            open.RemoveAt(0);
            close.Add(current);

            if (current.pos.x == endX && current.pos.y == endY)
            {
                // Renew everything for next use
                open.Clear();
                close.Clear();
                return current;
            }

            Vector2[] posList = new Vector2[4];
            posList[0] = new Vector2(current.pos.x - 1, current.pos.y);
            posList[1] = new Vector2(current.pos.x + 1, current.pos.y);
            posList[2] = new Vector2(current.pos.x, current.pos.y - 1);
            posList[3] = new Vector2(current.pos.x, current.pos.y + 1);
            int newG = current.Gcost + 1;
            foreach (Vector2 newPos in posList)
            {
                // Calculate neighbors if available
                if ((newPos.x >= 0 && newPos.x <= 29 && newPos.y >= 0 && newPos.y <= 15) 
                        && mapGrid[(int)newPos.x, (int)newPos.y] && CheckInClosed(newPos))
                {
                    Node inOpen = CheckInOpen(newPos);
                    // If neighbor isn't in open, add it
                    if (inOpen == null)
                    {
                        Node newNode = new Node(newG, Mathf.Abs(endX - startX) + Mathf.Abs(endY - startY), (int)newPos.x, (int)newPos.y);
                        newNode.parent = current;
                        open.Add(newNode);
                    }
                    // If the new path to neighbor is better, reassign
                    else if (inOpen.Gcost > newG)
                    {
                        inOpen.Gcost = newG;
                        inOpen.FCost = newG + inOpen.Hcost;
                        inOpen.parent = current;
                    }
                }
            }
            
        }
    }

    // Find a new available position if the current isn't
    private Vector2 CheckAvailable(Vector3 pos)
    {
        if (mapGrid[(int)pos.x, (int)pos.y])
        {
            return new Vector2(-1, -1);
        }
        // If this is terrain, then first seek the available block closest to it
        float xVal = pos.x % 1;
        float yVal = pos.y % 1;
        Vector2 closest;
        // Choose one axis value greater
        if (Mathf.Abs(xVal - 0.5f) >= Mathf.Abs(yVal - 0.5f))
        {
            if (xVal-0.5f > 0)
            {
                closest = new Vector2((int)pos.x+1, (int)pos.y);
            } else
            {
                closest = new Vector2((int)pos.x-1, (int)pos.y);
            }
        } else
        {
            if (yVal - 0.5f > 0)
            {
                closest = new Vector2((int)pos.x, (int)pos.y+1);
            }
            else
            {
                closest = new Vector2((int)pos.x, (int)pos.y-1);
            }
        }
        // If closest position is available, return it. If not, then randonly do one available
        if ((closest.x >= 0 && closest.x <= 29 && closest.y >= 0 && closest.y <= 15)
                        && mapGrid[(int)closest.x, (int)closest.y])
        {
            return closest;
        }
        else
        {
            Vector2 current = new Vector2((int)pos.x, (int)pos.y);
            Vector2[] posList = new Vector2[4];
            posList[0] = new Vector2(current.x - 1, current.y);
            posList[1] = new Vector2(current.x + 1, current.y);
            posList[2] = new Vector2(current.x, current.y - 1);
            posList[3] = new Vector2(current.x, current.y + 1);
            foreach (Vector2 newPos in posList)
            {
                // Calculate neighbors if available
                if ((newPos.x >= 0 && newPos.x <= 29 && newPos.y >= 0 && newPos.y <= 15) && mapGrid[(int)newPos.x, (int)newPos.y])
                {
                    return newPos;
                }
            }
        }
        return new Vector2(-1, -1);
    }

    // Find the node with lowest Fcost in open
    private Node FindLowestInOpen()
    {
        int minF = ((Node)open[0]).FCost;
        int pos = 0;
        for (int i = 1; i < open.Count; i++)
        {
            if (((Node)open[i]).FCost < minF)
            {
                minF = ((Node)open[i]).FCost;
                pos = i;
            }
        }
        return (Node)open[pos];
    }

    private bool CheckInClosed(Vector2 pos)
    {
        for (int i = 0; i < close.Count; i++)
        {
            if (((Node)close[i]).pos.Equals(pos))
            {
                return false;
            }
        }
        return true;
    }

    private Node CheckInOpen(Vector2 pos)
    {
        for (int i = 0; i < open.Count; i++)
        {
            if (((Node)open[i]).pos.Equals(pos))
            {
                return (Node)open[i];
            }
        }
        return null;
    }
}