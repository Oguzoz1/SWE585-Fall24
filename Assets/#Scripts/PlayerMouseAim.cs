using Player.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseAim : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.25f;        // Speed at which the spaceship rotates.
    [SerializeField] private float maxTiltAngle = 1.0f;          // Maximum tilt angle for banking.
    [SerializeField] private float deadZoneRadius = 50.0f;       // Radius for the dead zone around the screen center.

    private Vector3 screenCenter;
    private void Start()
    {
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        Cursor.visible = false;
    }

    private void FixedUpdate()
    {

        // Calculate mouse offset from the screen center.
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = mousePosition - screenCenter;

        // Check if the mouse offset is outside the dead zone.
        float offsetMagnitude = offset.magnitude;

        // Only rotate if the offset is outside the dead zone.
        if (offsetMagnitude > deadZoneRadius)
        {
            // Adjust the offset to exclude the dead zone.
            Vector3 adjustedOffset = (offset - offset.normalized * deadZoneRadius) / (Screen.width / 2f);

            // Calculate rotation angles.
            float pitch = Mathf.Clamp(adjustedOffset.y * rotationSpeed, -maxTiltAngle, maxTiltAngle);
            float yaw = Mathf.Clamp(adjustedOffset.x * rotationSpeed, -maxTiltAngle, maxTiltAngle);

            // Rotate the spaceship based on the adjusted offset.
            transform.Rotate(-pitch, yaw, 0);
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }

    // Draw the dead zone circle and mouse position dot.
    private void OnGUI()
    {
        // Calculate the screen position for the center and mouse.
        Vector2 deadZonePosition = new Vector2(screenCenter.x, Screen.height - screenCenter.y);
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

        // Draw the dead zone circle (white).
        Color originalColor = GUI.color;
        GUI.color = Color.white;
        GUI.DrawTexture(new Rect(deadZonePosition.x - deadZoneRadius, deadZonePosition.y - deadZoneRadius, deadZoneRadius * 2, deadZoneRadius * 2), CreateCircleTexture((int)(deadZoneRadius * 2), Color.white));

        // Draw the mouse position dot (blue).
        GUI.color = Color.blue;
        GUI.DrawTexture(new Rect(mousePosition.x - 5, mousePosition.y - 5, 10, 10), CreateCircleTexture(10, Color.blue));

        // Reset the color to avoid affecting other GUI elements.
        GUI.color = originalColor;
    }

    // Helper function to create a circular texture.
    private Texture2D CreateCircleTexture(int diameter, Color color)
    {
        Texture2D texture = new Texture2D(diameter, diameter, TextureFormat.ARGB32, false);
        Color[] colors = new Color[diameter * diameter];
        Vector2 center = new Vector2(diameter / 2, diameter / 2);
        float radius = diameter / 2f;

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                Vector2 pos = new Vector2(x, y);
                colors[x + y * diameter] = Vector2.Distance(pos, center) < radius ? color : Color.clear;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }
}
