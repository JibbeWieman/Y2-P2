using UnityEngine;

public class HeightAdjustment : MonoBehaviour
{
    //float direction = 0;

    [SerializeField]
    float speed;

    [SerializeField]
    private Transform cameraOffset;

    private float minHeight = 0.1f;
    private float maxHeight = 3.4f;

    //void Update()
    //{
    //    if (cameraOffset.position.y > minHeight && cameraOffset.position.y < maxHeight)
    //    {
    //        cameraOffset.position += new Vector3(0, direction * Time.deltaTime, 0);
    //    }
    //}

    public void GoUp(float amount)
    {
        //if (goUp)
        //    direction = 1 * speed;
        //else
        //    direction = 0;

        if (cameraOffset.position.y < maxHeight)
        {
            Debug.Log("Me go big");
            cameraOffset.position += new Vector3(0, amount, 0);
        }
    }

    public void GoDown(float amount)
    {
        //if (goDown)
        //    direction = -1 * speed;
        //else
        //    direction = 0;

        if (cameraOffset.position.y > minHeight)
        {
            Debug.Log("Me go smoll");
            cameraOffset.position += new Vector3(0, amount, 0);
        }
    }
}
