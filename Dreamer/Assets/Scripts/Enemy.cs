﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy {
    void TakeDamage(float damage);
    void Respawn();
}
