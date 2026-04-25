using Cainos.PixelArtPlatformer_Dungeon;
using UnityEngine;

public class DoorRange : MonoBehaviour
{
    public Door door;
    private void Start()
    {
        door = this.GetComponent<Door>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        door.Open();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        door.Close();
    }
}
