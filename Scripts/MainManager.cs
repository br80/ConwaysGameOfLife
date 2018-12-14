using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

  private int gridWidth;
  private int gridHeight;

  private Cell[,] cells;

  private bool[,] grid0;
  private bool[,] grid1;

  private bool currentGridIs0;
  private bool[,] currentGrid;
  private bool[,] otherGrid;

  private float lastChange = 0;

	// Use this for initialization
	void Start () {
    gridWidth = 15;
    gridHeight = 15;

    cells = new Cell[gridWidth, gridHeight];

    grid0 = new bool[gridWidth, gridHeight];
    grid1 = new bool[gridWidth, gridHeight];

    currentGridIs0 = true;
    currentGrid = grid0;

    transform.position = new Vector3((gridWidth - 1) / 2f, (gridHeight - 1) / 2f, -10);
    gameObject.GetComponent<Camera>().orthographicSize = Mathf.Max(gridWidth, gridHeight) / 2f;

    for (int i = 0 ; i < gridWidth ; i++) {
      for (int j = 0 ; j < gridHeight ; j++) {
        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cell.name = "Cell: " + i + ", " + j;
        cell.transform.position = new Vector3(i, j, 0);
        cell.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/UnlitMaterial");
        cells[i,j] = cell.AddComponent<Cell>();
      }
    }

    for (int i = 0 ; i < gridWidth ; i++) {
      for (int j = 0 ; j < gridHeight ; j++) {
        // if (Random.value > 0.5f)  {
        //   grid0[i,j] = true;
        //   grid1[i,j] = true;
        // }

        if ((i == 5 && j == 5) ||
            (i == 6 && j == 4) ||
            (i == 4 && j == 3) ||
            (i == 5 && j == 3) ||
            (i == 6 && j == 3)) {
          grid0[i,j] = true;
          grid1[i,j] = true;
        }
      }
    }


    updateCells();
	}

  private void tickGrid() {
    int total = 0;

    currentGridIs0 = !currentGridIs0;
    if (currentGridIs0) {
      currentGrid = grid0;
      otherGrid = grid1;
    } else {
      currentGrid = grid1;
      otherGrid  = grid0;
    }

    for (int i = 0 ; i < gridWidth ; i++) {
      for (int j = 0 ; j < gridHeight ; j++) {
        total = 0;

        if (j < gridHeight - 1 && otherGrid[i,j+1]) total++;

        if(j > 0 && otherGrid[i,j-1]) total++;

        if(i < gridWidth - 1 && otherGrid[i+1,j]) total++;

        if(i > 0 && otherGrid[i-1,j]) total++;

        if (j < gridHeight - 1 && otherGrid[i,j+1]) total++;


        if (j < gridHeight - 1 && i < gridWidth - 1 && otherGrid[i+1,j+1]) total++;

        if (j > 0 && i < gridWidth - 1 && otherGrid[i+1,j-1]) total++;

        if (j < gridHeight - 1 && i > 0 && otherGrid[i-1,j+1]) total++;

        if (j > 0 && i > 0 && otherGrid[i-1,j-1]) total++;


        if (currentGrid[i,j]) {  // Cell is alive
          if (total == 2 || total == 3) {
            currentGrid[i,j] = true;
          } else {
            currentGrid[i,j] = false;
          }
        } else {
          if (total == 3) {
            currentGrid[i,j] = true;
          } else {
            currentGrid[i,j] = false;
          }
        }

      }
    }
  }

  private void updateCells() {
    for (int i = 0 ; i < gridWidth ; i++) {
      for (int j = 0 ; j < gridHeight ; j++) {
        if (currentGrid[i,j]) {
          cells[i,j].Revive();
        } else {
          cells[i,j].Kill();
        }
      }
    }
  }

  void Update() {
    lastChange += Time.deltaTime;
    if (lastChange > 1) {
      lastChange -= 1;
      tickGrid();
      updateCells();
    }
  }

}
