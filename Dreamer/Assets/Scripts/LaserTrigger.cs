using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour {

    public List<Laser> lasers = new List<Laser>();
    public int laserCount;

    void Update() {
        if (lasers.Count == laserCount) {
            print("jee");
        }        
    }

    public void AddLaserToList(Laser lzr) {
        if (lasers.Contains(lzr)) {
            return;
        } else {
            lasers.Add(lzr);
        }
    }

    public void RemoveLaserFromList(Laser lzr) {
        if (lasers.Contains(lzr)) {
            lasers.Remove(lzr);
        } else {
            return;
        }
    }
}
