﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeHit(int damage);

    float GetArmorStat();
}
