using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

  public bool alive;

  public void Kill() {
    if (alive) {
      alive = false;
      gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
  }

  public void Revive() {
    if (!alive) {
      alive = false;
      gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
    }
  }


}
