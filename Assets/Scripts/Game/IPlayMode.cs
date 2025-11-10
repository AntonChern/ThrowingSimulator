using UnityEngine;

public interface IPlayMode
{
    public void SendCrateInteracting(string id, int index, bool isTaken);
    public void SendCrateMoving(int index);
    public void SendCrateVelocity(int index);
    public void SendPlayerLocation(string id);
    public void SendPlayerMovement(string id, Vector3 movement);
    public void SendCrateAuthority(string id, int index);
    public void Exit();
}
