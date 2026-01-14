using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LunaBehaviourScript : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    public float speed = 7f;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");
        Vector3 Position = transform.position;
        Position.x = Position.x + horizontal * Time.deltaTime * speed;
        Position.y = Position.y + vertical * Time.deltaTime * speed;
        rigidbody2D.MovePosition(Position);
    }
}
