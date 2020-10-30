using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float AddPowerValue;

    public void HandleDestroy()
    {
        GameManager.Instance.MapGenerator.DeleteCoin(this);
        Destroy(this.gameObject);
    }
}
