using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 100f;
    public float thrust = 150f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 motion = transform.right * x + transform.forward * z;
        controller.Move(motion * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
            controller.Move(Vector3.up * thrust * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            controller.Move(Vector3.up * -thrust * Time.deltaTime);
    }
}
