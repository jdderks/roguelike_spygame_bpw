using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variables that are set by the user in the inspector
    [SerializeField] private float movementSpeed = 2.5f;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject impactEffects;
    [SerializeField] private GameObject flashLight;
    //Variables that are set in start
    private Rigidbody2D rb2d;
    private LineRenderer line;
    private Animator anim;


    private FieldOfView fieldOfView;
    [SerializeField] private LayerMask seenLayer;

    //Global variables
    private Vector2 movement;
    private Vector2 mousePosition;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        anim = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (flashLight.activeInHierarchy)
            {
                flashLight.SetActive(false);
            } 
            else
            {
                flashLight.SetActive(true);
            }
        }

        if (flashLight.activeInHierarchy)
        {
            SetVisibleWallsToSeen();
        }
    }

    private void SetVisibleWallsToSeen()
    {
        for (int i = 0; i < fieldOfView.visibleTargets.Count; i++)
        {
            Debug.Log(fieldOfView.visibleTargets[i]);
            fieldOfView.visibleTargets[i].GetComponent<Wall>().DiscoverWall();
        }
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + movement.normalized * movementSpeed * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePosition - rb2d.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb2d.rotation = angle;
    }

    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up),Mathf.Infinity,~LayerMask.GetMask("Player"));
        if (hit)
        {
            GameObject ip = Instantiate(impactEffects, hit.point, Quaternion.identity);
            Destroy(ip,0.1f);
            anim.SetTrigger("ShootLaser");
            line.SetPosition(0,transform.position + transform.TransformDirection(Vector2.up - new Vector2(0, 0.7f)));
            line.SetPosition(1,hit.point);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position,transform.up);
    }
}
