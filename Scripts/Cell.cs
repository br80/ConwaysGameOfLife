using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

  MainManager mainManager;

  private int i;
  private int j;
  private int k;

  public void Initialize(MainManager setMainManager, int setI, int setJ, int setK) {
    mainManager = setMainManager;

    i = setI;
    j = setJ;
    k = setK;
  }

  void Update() {
    if (mainManager.GetLife(i, j, k)) {
      gameObject.GetComponent<MeshRenderer>().enabled = true;
    } else {
      gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
  }


}
