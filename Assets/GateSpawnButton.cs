using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GateSpawnButton : MonoBehaviour
{
    public GATEbase GateToSpawn;
    [SerializeField] GATEbase spawnedGate;
    [SerializeField] Image gateImage;
    [SerializeField] static bool spawned;
    [SerializeField] public TextMeshProUGUI title;

    private void Start()
    {
        title.text = GateToSpawn.Name;
    }
    public void SpawnGate()
    {
        if (spawned) return;
        spawnedGate = Instantiate(GateToSpawn);
        spawned = true;
    }

    private void Update()
    {
        if (!spawned || !spawnedGate) return;
        spawnedGate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).Change(0,0,10);
        if (Input.GetMouseButtonDown(0))
        {
            spawned = false;
            spawnedGate = null;
        }
    }
}
