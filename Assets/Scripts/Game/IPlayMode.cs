using UnityEngine;

public interface IPlayMode
{
    public void SendCrateInteracting(string id, int index, bool isTaken);
    public void SendCrateMoving(int index);
    public void SendPlayerMoving(string id);
    public void SendCrateAuthority(string id, int index);
    public void Exit();
}
