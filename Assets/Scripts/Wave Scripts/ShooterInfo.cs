using UnityEngine;

[System.Serializable]
public class ShooterInfo {
    public Vector3 startPos, onScreen, end;
    public float waitTimer, startToScreenSpeed, onScreenTime, screenToEndSpeed;

    public void getScript(GameObject shooter) {
        shooter.GetComponent<Shooters>().setShooters(startPos, onScreen, end, startToScreenSpeed, onScreenTime, screenToEndSpeed, waitTimer);
    }
}
