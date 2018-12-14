using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MainManager : MonoBehaviour {

  private int gridWidth;
  private int gridHeight;
  private int gridDepth;

  private Cell[,,] cells;

  private bool[,,] grid0;
  private bool[,,] grid1;

  private bool currentGridIs0;
  private bool[,,] currentGrid;
  private bool[,,] otherGrid;

  private float lastChange = 0;

  private float tickLength = 0.2f;
  private float currentTime = 0;

  private float startingDensity = 0.5f;

  private bool hasStarted = false;

  private GameObject lookAt;

	// Use this for initialization
	void Start () {

    int dimension = 50;

    gridWidth = dimension;
    gridHeight = dimension;
    gridDepth = dimension;

    cells = new Cell[gridWidth, gridHeight, gridDepth];

    grid0 = new bool[gridWidth, gridHeight, gridDepth];
    grid1 = new bool[gridWidth, gridHeight, gridDepth];

    currentGridIs0 = true;
    currentGrid = grid0;
    otherGrid = grid1;

    transform.position = new Vector3((gridWidth - 1) / 2f + gridWidth, (gridHeight - 1) / 2f + gridHeight, -(gridDepth - 1));
    gameObject.GetComponent<Camera>().orthographicSize = Mathf.Max(gridWidth, gridHeight) / 2f;

    lookAt = new GameObject();
    lookAt.transform.position = new Vector3((gridWidth - 1) / 2f, (gridHeight - 1) / 2f, (gridDepth - 1) / 2f);

    for (int i = 0 ; i < gridWidth ; i++) {
      for (int j = 0 ; j < gridHeight ; j++) {
        for (int k = 0 ; k < gridDepth ; k++) {
          GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
          cell.name = "Cell: " + i + ", " + j + ", " + k;
          cell.transform.position = new Vector3(i, j, k);
          cell.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
          cell.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/UnlitMaterial");
          cell.GetComponent<MeshRenderer>().material.SetColor ("_Color", new Color((1f * i) / gridWidth, (1f * j) / gridHeight, (1f * k) / gridDepth ));
          cells[i,j,k] = cell.AddComponent<Cell>();
          cell.GetComponent<Cell>().Initialize(this, i, j, k);

          Destroy(cell.GetComponent<BoxCollider>());
        }
      }
    }

    for (int i = 0 ; i < gridWidth ; i++) {
      for (int j = 0 ; j < gridHeight ; j++) {
        for (int k = 0 ; k < gridDepth ; k++) {
          if (Random.value > startingDensity)  {
            currentGrid[i,j,k] = true;
            otherGrid[i,j,k] = true;
          }
        }
      }
    }


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
        for (int k = 0 ; k < gridDepth ; k++) {
          total = 0;

          if (j < gridHeight - 1 && otherGrid[i,j+1,k]) total++;
          if(j > 0 && otherGrid[i,j-1,k]) total++;
          if(i < gridWidth - 1 && otherGrid[i+1,j,k]) total++;
          if(i > 0 && otherGrid[i-1,j,k]) total++;

          if (j < gridHeight - 1 && i < gridWidth - 1 && otherGrid[i+1,j+1,k]) total++;
          if (j > 0 && i < gridWidth - 1 && otherGrid[i+1,j-1,k]) total++;
          if (j < gridHeight - 1 && i > 0 && otherGrid[i-1,j+1,k]) total++;
          if (j > 0 && i > 0 && otherGrid[i-1,j-1,k]) total++;

          if (k > 0) {
            if (j < gridHeight - 1 && otherGrid[i,j+1,k-1]) total++;
            if(j > 0 && otherGrid[i,j-1,k-1]) total++;
            if(i < gridWidth - 1 && otherGrid[i+1,j,k-1]) total++;
            if(i > 0 && otherGrid[i-1,j,k-1]) total++;

            if (j < gridHeight - 1 && i < gridWidth - 1 && otherGrid[i+1,j+1,k-1]) total++;
            if (j > 0 && i < gridWidth - 1 && otherGrid[i+1,j-1,k-1]) total++;
            if (j < gridHeight - 1 && i > 0 && otherGrid[i-1,j+1,k-1]) total++;
            if (j > 0 && i > 0 && otherGrid[i-1,j-1,k-1]) total++;

            if (j > 0 && i > 0 && otherGrid[i,j,k-1]) total++;
          }

          if (k < gridDepth - 1) {
            if (j < gridHeight - 1 && otherGrid[i,j+1,k+1]) total++;
            if(j > 0 && otherGrid[i,j-1,k+1]) total++;
            if(i < gridWidth - 1 && otherGrid[i+1,j,k+1]) total++;
            if(i > 0 && otherGrid[i-1,j,k+1]) total++;

            if (j < gridHeight - 1 && i < gridWidth - 1 && otherGrid[i+1,j+1,k+1]) total++;
            if (j > 0 && i < gridWidth - 1 && otherGrid[i+1,j-1,k+1]) total++;
            if (j < gridHeight - 1 && i > 0 && otherGrid[i-1,j+1,k+1]) total++;
            if (j > 0 && i > 0 && otherGrid[i-1,j-1,k+1]) total++;

            if (j > 0 && i > 0 && otherGrid[i,j,k+1]) total++;
          }

          if (otherGrid[i,j,k]) {  // Cell is alive
            currentGrid[i,j,k] = total == 2 || total == 3;
          } else {
            currentGrid[i,j,k] = total == 3;
          }

        }
      }
    }
  }

  public bool GetLife(int i, int j, int k) {
    return currentGrid[i,j,k];
  }

  // void Update() {
  //   if (Input.GetMouseButtonDown(0)) {
  //     Debug.Log("CLICK");
  //     tickGrid();
  //   }
  // }

  void FixedUpdate() {

    if (hasStarted) {
      currentTime += Time.deltaTime;
      if (currentTime > tickLength) {
        currentTime -= tickLength;
        tickGrid();
      }
    } else {
      if (Input.GetKeyDown(KeyCode.Return)) {
        hasStarted = true;
      }
    }

    transform.LookAt(lookAt.transform);
    transform.Translate(Vector3.right * Time.deltaTime * 50f);


  }

}
