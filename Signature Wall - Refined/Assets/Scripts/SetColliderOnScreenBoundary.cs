using UnityEngine;

public class SetColliderOnScreenBoundary : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        //get screen boundary
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 bottomRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        Vector2 topLeft = cam.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //add edge collider
        EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D>();
        edge.points = new Vector2[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
    }
}
