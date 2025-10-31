using Assets.Player.Scripts;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player: MonoBehaviour
{
    public PlayerStats playerStats;

    public PlayerStats runtimeStats;

    private Vector2 _windowSize = new Vector2(Screen.width, Screen.height);

    private Rigidbody _rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.runtimeStats = playerStats;
        _rigidbody = this.GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateShots();
    }

    void UpdateMovement()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 MousePositionCentered = mousePosition - (_windowSize / 2);


        if (playerStats != null)
        {
            _rigidbody.linearVelocity = moveDirection * runtimeStats.moveSpeed;
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(MousePositionCentered.x, MousePositionCentered.y) * Mathf.Rad2Deg, 0);
        }
    }

    void UpdateShots()
    {
        if (Input.GetMouseButtonDown(0))
        {
            System.Diagnostics.Debug.WriteLine("Shoot!");
            GameObject missile = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), transform.position + (transform.rotation * new Vector3(0, 0, 2)), transform.rotation);
            missile.AddComponent<Missile>();
        }
    }
}
