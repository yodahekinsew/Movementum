using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Controls
{
    Joystick,
    Tilt,
    Drag
}

public class Player : MonoBehaviour
{
    public static Controls controlScheme = Controls.Drag;

    public GameManager gameManager;
    public SpriteRenderer sprite;
    public CircleCollider2D collider;

    public float speedMultiplier;
    public Transform shadow;

    public Animator anim;


    [Header("Controls")]
    public Joystick joystick;

    [Header("Shooting")]
    public BulletPool bulletPool;
    public float minShotRate;
    public float maxShotRate;
    public AudioClip shotEffect;
    private float shootingCooldown = 1;

    [Header("Streaks")]
    public StreakCounter streak;
    public float streakMovementClamp;
    public float streakIncreaseRate;
    public float streakDecreaseRate; // when not moving, streak decreases by constant float

    [Header("Spike Powerup")]
    public GameObject spikeRing;
    public bool hasSpike;

    [Header("Shield Powerup")]
    public Shield shield;
    public bool hasShield;

    private float halfWidth;
    private float halfHeight;

    private Gyroscope gyro;
    private Vector3 calibrationTilt = Vector3.zero;

    private bool controlActivated;

    private bool invincible = false;
    private bool killed = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Controls")) PlayerPrefs.SetInt("Controls", 0);
        switch (PlayerPrefs.GetInt("Controls"))
        {
            case 0:
                controlScheme = Controls.Joystick;
                break;
            case 1:
                controlScheme = Controls.Tilt;
                break;
            case 2:
                controlScheme = Controls.Drag;
                break;
        }

        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;

        gyro = Input.gyro;
        gyro.enabled = true;
        if (!PlayerPrefs.HasKey("CalibrationTiltX"))
        {
            PlayerPrefs.SetFloat("CalibrationTiltX", 0.0f);
            PlayerPrefs.SetFloat("CalibrationTiltY", 0.0f);
        }
        calibrationTilt = new Vector3(
            PlayerPrefs.GetFloat("CalibrationTiltX"),
            PlayerPrefs.GetFloat("CalibrationTiltY"),
            0
        );
    }

    private bool MouseInDeadZone()
    {
        Vector3 screenMousePosition = Input.mousePosition;
        if (screenMousePosition.x > .85f * Screen.width
            && screenMousePosition.x < .95f * Screen.width
            && screenMousePosition.y < .1f * Screen.height) return true;
        return false;
    }

    void Update()
    {
        if (!controlActivated)
        {
            if (!killed && GameManager.state != GameState.Pause) transform.position = Vector3.Lerp(
                transform.position,
                Vector3.zero,
                .5f * speedMultiplier * Time.deltaTime
            );
            return;
        }

        Vector3 oldPosition = transform.position;
        switch (controlScheme)
        {
            case Controls.Drag:
                // Moving by following player finger
                if (Input.GetMouseButton(0))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;
                    transform.up = (mousePosition - transform.position).normalized;
                    transform.position = Vector3.Lerp(transform.position, mousePosition, speedMultiplier / 2f * Time.deltaTime);
                }
                break;
            case Controls.Joystick:
                // Moving by joystick
                Vector3 joystickDir = joystick.GetDir();
                Vector3 moveDir = joystickDir.normalized;
                float moveAmount = joystickDir.magnitude;

                transform.up = moveDir;
                transform.position += moveDir * moveAmount * speedMultiplier * Time.deltaTime;
                break;
            case Controls.Tilt:
                // **** Moving by tilt ******
                Vector3 tilt = Input.acceleration - calibrationTilt;
                Vector3 moveVector = new Vector3(
                    tilt.x * 5 * speedMultiplier,
                    tilt.y * 5 * speedMultiplier,
                    0
                );
                transform.up = moveVector.normalized;
                transform.position += moveVector * Time.deltaTime;
                break;
        }

        // Constrain player in screen
        if (transform.position.x < -halfWidth + .5f) transform.position = new Vector3(-halfWidth + .5f, transform.position.y, transform.position.z);
        if (transform.position.x > halfWidth - .5f) transform.position = new Vector3(halfWidth - .5f, transform.position.y, transform.position.z);
        if (transform.position.y < -halfHeight + .5f) transform.position = new Vector3(transform.position.x, -halfHeight + .5f, transform.position.z);
        if (transform.position.y > halfHeight - .5f) transform.position = new Vector3(transform.position.x, halfHeight - .5f, transform.position.z);

        if (GameManager.state != GameState.Play) return;

        float distanceMoved = Vector3.Distance(transform.position, oldPosition);
        shootingCooldown -= Mathf.Lerp(minShotRate, maxShotRate, GameManager.difficulty) * Mathf.Pow(distanceMoved, 2);
        if (shootingCooldown <= 0)
        {
            Shoot();
            shootingCooldown = 1;
        }

        // Update streak based on the player's movement
        float streakChange = -streakDecreaseRate * Time.deltaTime;
        float maxDistance = streakMovementClamp * speedMultiplier * Time.deltaTime;
        float clampedDistance = Mathf.Clamp(distanceMoved, 0, maxDistance);
        streakChange += streakIncreaseRate * (clampedDistance / maxDistance) * Time.deltaTime;
        streak.FillStreak(streakChange);

        shadow.position = transform.position + new Vector3(.1f, -.1f, .1f);

        shield.transform.eulerAngles = Vector3.zero;
    }

    public void SetInvincible(bool invincibility)
    {
        invincible = invincibility;
    }

    void Shoot()
    {
        GameObject newBullet = bulletPool.GetBullet();
        newBullet.transform.position = transform.position - transform.up;
        newBullet.transform.up = -transform.up;
        newBullet.GetComponent<Bullet>().Fire();
        Vibration.VibratePeek();
        // AudioManager.instance.PlaySFX(shotEffect, 0.5f);
    }

    public void Kill()
    {
        if (!killed && !invincible)
        {
            killed = true;
            // DeactivateSpike();
            DeactivateControls();
            anim.SetTrigger("Explode");
            Vibration.VibrateNope();
            gameManager.EndGame();
        }
    }

    public void FinishExploding()
    {
        transform.position = Vector3.zero;
    }

    public void CalibrateTilt()
    {
        calibrationTilt = new Vector3(
            Input.acceleration.x,
            Input.acceleration.y,
            0
        );
        PlayerPrefs.SetFloat("CalibrationTiltX", Input.acceleration.x);
        PlayerPrefs.SetFloat("CalibrationTiltY", Input.acceleration.y);
    }

    public void SwitchToJoystick()
    {
        controlScheme = Controls.Joystick;
        PlayerPrefs.SetInt("Controls", 0);
        if (controlActivated) joystick.Activate();
    }

    public void SwitchToTilt()
    {
        if (controlScheme == Controls.Joystick) joystick.Deactivate();
        controlScheme = Controls.Tilt;
        PlayerPrefs.SetInt("Controls", 1);
    }

    public void SwitchToDrag()
    {
        if (controlScheme == Controls.Joystick) joystick.Deactivate();
        controlScheme = Controls.Drag;
        PlayerPrefs.SetInt("Controls", 2);
    }

    public void ActivateControls()
    {
        controlActivated = true;
        if (controlScheme == Controls.Joystick) joystick.Activate();
    }

    public void ActivateShield()
    {
        hasShield = true;
        shield.Show();
    }

    public void DeactivateShield()
    {
        hasShield = false;
        shield.Hide();
    }

    public void ActivateSpike()
    {
        hasSpike = true;
        spikeRing.SetActive(true);
        // collider.radius = .75f;
    }

    public void DeactivateSpike()
    {
        spikeRing.SetActive(false);
        // collider.radius = .5f;
        if (killed) hasSpike = false;
        else StartCoroutine(RemoveSpike());
    }

    public IEnumerator RemoveSpike()
    {
        yield return new WaitForSeconds(.2f);
        hasSpike = false;
    }

    public void Appear()
    {
        anim.SetTrigger("Appear");
        killed = false;
    }

    public void DeactivateControls()
    {
        controlActivated = false;
        if (controlScheme == Controls.Joystick) joystick.Deactivate();
    }
}
