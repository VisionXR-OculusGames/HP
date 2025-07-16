using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCanvas : MonoBehaviour
{
    public List<GameObject> panels;


    private void OnDisable()
    {
        foreach(GameObject go in panels)
        {
            go.SetActive(false);
        }
    }
}
