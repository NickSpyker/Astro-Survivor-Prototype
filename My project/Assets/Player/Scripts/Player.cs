using Assets.Player.Scripts;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class Player: MonoBehaviour
{
    public PlayerStats playerStats;

    private PlayerStats runtimeStats;

    private Vector2 _windowSize = new Vector2(Screen.width, Screen.height);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runtimeStats = playerStats;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateShots();
    }

    void UpdateMovement()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 MousePositionCentered = mousePosition - (_windowSize / 2);


        if (playerStats != null)
        {
            transform.position += moveDirection * runtimeStats.moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(MousePositionCentered.x, MousePositionCentered.y) * Mathf.Rad2Deg, 0);
        }
    }

    void UpdateShots()
    {
        if (Input.GetMouseButtonDown(0))
        {
            System.Diagnostics.Debug.WriteLine("Shoot!");
            this.AddComponent<Missile>();
            this.GetComponent<Missile>().position = transform.position + transform.forward * 1.5f;
            this.GetComponent<Missile>().direction = transform.forward;
            this.GetComponent<Missile>().speed = 10f;
        }
    }
}
