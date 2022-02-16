using UnityEngine;

[System.Serializable]
public class DeflectorInfo
{
    public Vector2 startPos, pos1, pos2;
    public float velocity;

    public void getScript(GameObject obj) {
        obj.GetComponent<Deflectors>().setDeflectors(startPos, pos1, pos2, velocity);
    }
}
