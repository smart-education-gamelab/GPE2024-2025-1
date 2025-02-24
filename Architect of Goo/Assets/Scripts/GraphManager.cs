using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public GameObject player;
    public GameObject xGraph;
    public GameObject yGraph;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
        Debug.Log(camera.orthographicSize);
    }

    private void FixedUpdate()
    {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        xGraph.transform.position = new Vector2(xGraph.transform.position.x, playerY - camera.orthographicSize+3);
        yGraph.transform.position = new Vector2(playerX - (camera.orthographicSize*1.5f), yGraph.transform.position.y);
    }
}
