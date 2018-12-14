using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

  private int gridWidth;
  private int gridHeight;

  private Cell[,] grid;

	// Use this for initialization
	void Start () {
    gridWidth = 15;
    gridHeight = 15;

    grid = new Cell[gridWidth, gridHeight];

    transform.position = new Vector3((gridWidth - 1) / 2f, (gridHeight - 1) / 2f, -10);
    gameObject.GetComponent<Camera>().orthographicSize = Mathf.Max(gridWidth, gridHeight) / 2f;

    for (int i = 0 ; i < gridWidth ; i++) {
      for (int j = 0 ; j < gridHeight ; j++) {
        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cell.name = "Cell: " + i + ", " + j;
        cell.transform.position = new Vector3(i, j, 0);
        cell.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/UnlitMaterial");
        grid[i,j] = cell.AddComponent<Cell>();
        // if (Random.value > 0.5f) {
        //   grid[i,j].Kill()
        // }
      }
    }
	}

}
