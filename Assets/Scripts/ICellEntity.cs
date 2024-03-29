﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICellEntity {

    Cell Cell { get; set; }

    GameObject gameObject { get; set; }

    // Returning false on execute action would mean that the player can't move into the next cell
    bool ExecuteAction();

    void Found();

    void HasFinishedMoving();

    bool ExecuteActionEntity(ICellEntity entity);

}
