using UnityEngine;

public class UserDirection : MonoBehaviour
{

    public GameObject cam;

    // Update is called once per frame
    void Update()
    {
        
        gameObject.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
        gameObject.transform.position = cam.transform.position;
    }

}
