using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private const float cameraSpeed = 0.5f;
    private const float viewDistance = 10.0f;

    public GameObject cameraRig;
    public GameObject handLeft;
    public GameObject sphere;

    public Transform sphereTrans;
    public Transform[ ] linePoints;

    public NavMeshAgent navMeshAgent;
    public NavMeshHit navMeshHit;
   
    private LineRenderer lineRenderer;

    private Vector3 offset;
    private Quaternion target;
    private Vector3 height = new Vector3( 0, 5000, -1000 );

    private float x;
    private float y;
	// Use this for initialization
	void Start () {
	   
        sphereTrans = GameObject.Find( "Sphere" ).transform;
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>( );

        x = Input.GetAxis( "Vertical" );
        y = Input.GetAxis( "Horizontal" );

        lineRenderer = gameObject.AddComponent<LineRenderer>( );
       
	}
	
	// Update is called once per frame
	void Update () {

        lineRenderer = gameObject.GetComponent<LineRenderer>( );

        //カメラの移動//
        x += Input.GetAxis( "Vertical" );
        y += Input.GetAxis( "Horizontal" );
        target = Quaternion.Euler(  -x * cameraSpeed, y * cameraSpeed, 0 );
        Camera.main.transform.rotation = Quaternion.Slerp( Camera.main.transform.rotation, target, Time.time ); 

        //----------------------------//	   
        //--------VR入力--------------//
        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
        var device = SteamVR_Controller.Input((int) trackedObject.index);

        //---------------------------//

        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        offset = sphereTrans.position - Camera.main.transform.position + height;

	    if ( Physics.Raycast( ray, out hitInfo ) ) {
            
            sphereTrans.position = hitInfo.point;

            Debug.Log( hitInfo.normal );
  
            if( hitInfo.normal.y < 0.1 ) {
               
                lineRenderer.material.color = Color.red;
                sphere.GetComponent<Renderer>( ).material.color = Color.red;
                Debug.DrawLine( linePoints[ 0 ].position, linePoints[ 1 ].position, Color.red );
            } else {
                
                lineRenderer.material.color = Color.green;
                sphere.GetComponent<Renderer>( ).material.color = Color.green;
                Debug.DrawLine( linePoints[ 0 ].position, linePoints[ 1 ].position, Color.green );
                if ( Input.GetMouseButtonDown( 0 ) || device.GetPressDown( SteamVR_Controller.ButtonMask.Trigger ) ) {
                    cameraRig.transform.position = hitInfo.point + offset.normalized * viewDistance;
                    Debug.Log( "トリガーを深く引いた" );
                }   
            }
            lineRenderer.SetPosition( 0, linePoints[ 0 ].position );
            lineRenderer.SetPosition( 1, linePoints[ 1 ].position );
            lineRenderer.SetWidth( 0.1f, 0.1f ); 
        }    
	}
}



