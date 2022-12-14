using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShip 
{
    void UpdateMovement();
    void UpdateRotation();
    void ShootCycle();
    void DoDamage(int damage);
    void OnDeath();
}
