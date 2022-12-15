public interface IShip 
{
    void UpdateMovement();
    void UpdateRotation();
    void ShootCycle();
    void DoDamage(int damage);
    void OnDeath();
}
