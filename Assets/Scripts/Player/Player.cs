using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player INSTANCE;

    public string gamename;
    public int permissionLevel;

    [Header("Controls")]
    public KeyCode up;
    public KeyCode left;
    public KeyCode down;
    public KeyCode right;

    [Header("Movement")]
    public float cameraFollowSpeed;
    public float speed;
    public float maxVelocityChange;
    public float jumpHeight;
    public float gravityMultiplier;
    public LayerMask groundLayers;

    //[HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isLeftGrounded;
    [HideInInspector]
    public bool isRightGrounded;

    [Header("Player States")]
    public bool disableControl;

    private Rigidbody2D rb;
    

    private float overrideSpeedMultiplier = 1;
    private float overrideJumpMultiplier = 1;

    private bool detectingSideHole = true;

    private void Start()
    {
        int sr = Screen.resolutions.Length - 1;
        Screen.SetResolution(Screen.resolutions[sr].width, Screen.resolutions[sr].height, true);
        rb = GetComponent<Rigidbody2D>();
        INSTANCE = this;
    }

    void Update()
    {
        U_Movement();
        //U_SideHoleDetection();
        U_MissingChunkFreeze();
    }

    void FixedUpdate()
    {
        FU_Movement();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + Vector3.back, cameraFollowSpeed * Time.deltaTime);
    }

    private void U_SideHoleDetection()
    {
        if (detectingSideHole)
        {
            if (Vector2.Distance(GetCoordinates(), GetTileCoordinates()) <= 0.1f)
            {
                bool pushing = false;
                int direction = 1;

                if (Input.GetKey(left) && !isGrounded && rb.velocity.x >= -0.01f)
                {
                    pushing = true;
                    direction = -1;
                }
                else if (Input.GetKey(right) && !isGrounded && rb.velocity.x <= 0.01f)
                {
                    pushing = true;
                    direction = 1;
                }

                if (pushing)
                {
                    Tile dirTile = Universe.INSTANCE.activePlanet.GetForeTile(GetTileCoordinates() + Vector2Int.right * direction);
                    Tile dirAndUpTile = Universe.INSTANCE.activePlanet.GetForeTile(GetTileCoordinates() + Vector2Int.right * direction + Vector2Int.up);
                    Tile dirAndDownTile = Universe.INSTANCE.activePlanet.GetForeTile(GetTileCoordinates() + Vector2Int.right * direction + Vector2Int.down);
                    Tile upTile = Universe.INSTANCE.activePlanet.GetForeTile(GetTileCoordinates() + Vector2Int.up);
                    Tile downTile = Universe.INSTANCE.activePlanet.GetForeTile(GetTileCoordinates() + Vector2Int.down);

                    if (dirTile != null && !(dirTile is TileMain && ((TileMain)dirTile).isCollidable))
                    {
                        if ((rb.velocity.y < -0.1f && dirAndDownTile != null && (dirAndDownTile is TileMain && ((TileMain)dirAndDownTile).isCollidable) && downTile != null && !(downTile is TileMain && ((TileMain)downTile).isCollidable)) || (rb.velocity.y > 0.1f && dirAndUpTile != null && (dirAndUpTile is TileMain && ((TileMain)dirAndUpTile).isCollidable) && upTile != null && !(upTile is TileMain && ((TileMain)upTile).isCollidable)))
                        {
                            Debug.Log("bur");
                            GoToCoordinates(GetCoordinates().x + (0.07f * direction), GetCoordinates().y);
                            StartCoroutine(CR_TripSideHoleDetection());
                        }
                    }
                }
            }
        }
    }

    private IEnumerator CR_TripSideHoleDetection()
    {
        detectingSideHole = false;
        yield return new WaitForSeconds(0.1f);
        detectingSideHole = true;
    }

    private Chunk rightChunk;
    private Chunk leftChunk;
    private Chunk topChunk;
    private Chunk bottomChunk;

    private Vector2 storedVelocity;

    private static Vector2Int[] directions = new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    private void U_MissingChunkFreeze()
    {
        rightChunk = Universe.INSTANCE.activePlanet.GetChunk(GetTileCoordinates() + directions[0]);
        leftChunk = Universe.INSTANCE.activePlanet.GetChunk(GetTileCoordinates() + directions[1]);
        topChunk = Universe.INSTANCE.activePlanet.GetChunk(GetTileCoordinates() + directions[2]);
        bottomChunk = Universe.INSTANCE.activePlanet.GetChunk(GetTileCoordinates() + directions[3]);

        if (!rb.isKinematic && (rightChunk == null || leftChunk == null || topChunk == null || bottomChunk == null || !rightChunk.IsLoaded() || !leftChunk.IsLoaded() || !topChunk.IsLoaded() || !bottomChunk.IsLoaded()))
        {
            //storedVelocity = rb.velocity;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
        else if (rb.isKinematic)
        {
            rb.isKinematic = false;
            //rb.velocity = storedVelocity;
        }
    }

    private void U_Movement()
    {
        if (!disableControl && Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector3.up * jumpHeight * overrideJumpMultiplier, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (!rb.isKinematic && rb.gravityScale > 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    private Vector3 targetVelocity;
    private Vector3 velocityChange;


    private void FU_Movement()
    {
        int horzAxis = Input.GetKey(left) ? -1 : Input.GetKey(right) ? 1 : 0;

        targetVelocity = disableControl ? Vector3.zero : Vector3.ClampMagnitude(new Vector3(horzAxis, 0, 0), 1);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed * overrideSpeedMultiplier;

        Vector3 velocity = rb.velocity;
        velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        rb.AddForce(velocityChange * (isGrounded ? 12 : 3.5f), ForceMode2D.Force);
    }

    public void GoToCoordinates(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    public Vector2 GetCoordinates()
    {
        return transform.position;
    }

    public Vector2Int GetTileCoordinates()
    {
        return new Vector2Int((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y));
    }

    public Vector2Int GetChunkCoordinates()
    {
        Vector2Int tc = GetTileCoordinates();
        return new Vector2Int(tc.x - tc.x.mod(Planet.chunkSize), tc.y - tc.y.mod(Planet.chunkSize));
    }
    
    public Vector2 GetChunkCenterCoordinates()
    {
        Vector2Int cc = GetChunkCoordinates();
        return new Vector2((float)cc.x + Planet.chunkSize / 2, (float)cc.y + Planet.chunkSize / 2);
    }

    public void SetSpeed(float newSpeed)
    {
        overrideSpeedMultiplier = newSpeed;
    }

    public void SetJumpHeight(float newJumpHeight)
    {
        overrideJumpMultiplier = newJumpHeight;
    }

    public static Player getLocalPlayer()
    {
        //TODO Return local player
        return INSTANCE;
    }
}
