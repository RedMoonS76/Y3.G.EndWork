using Cainos.PixelArtPlatformer_Dungeon;
using UnityEngine;

public class ChestCheck : MonoBehaviour
{
    public bool isClear = false;
    public bool isStay = false;
    public GameObject Pos1;
    public GameObject Pos2;
    public GameObject Pos3;
    private GameObject emeny;
    private Chest chest;
    private void Start()
    {
        chest = this.GetComponent<Chest>();
    }
    private void Update()
    {
        emeny = GameObject.FindWithTag("Enemy");
        if (emeny == null)
        {
            isClear = true;

        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            chest.Open();
            Pos1.SetActive(true);
            Pos2.SetActive(true);
            Pos3.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isClear)
            {
                isStay = true;
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isClear)
            {
                isStay = false;
            }
        }

    }
}
