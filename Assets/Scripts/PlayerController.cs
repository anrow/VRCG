using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private const float cameraSpeed = 0.5f;
    private const float viewDistance = 10.0f;
    private const int maxColor = 2;
    public GameObject cameraRig;
    public GameObject handLeft;
    public GameObject sphere;

    public Transform sphereTrans;
    public Transform[ ] linePoints;

    private int[ ] colors = new int[ maxColor ]; 
    private Color[ ] color = { Color.red, Color.green };
    private enum COLOR {
        red,
        green
    }
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

        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        offset = sphereTrans.position - Camera.main.transform.position + height;

	    if ( Physics.Raycast( ray, out hitInfo ) ) {
            
            //navMeshAgent.SetDestination( hitInfo.point );
            
            sphereTrans.position = hitInfo.point;

            Debug.Log( hitInfo.normal );
            
         
            for( int i = 0; i < colors.Length; ++i ) {
                if( hitInfo.normal.y < 0.1 ) {
                    i = colors[ (int)COLOR.red ];
                } else {
                    i = colors[ (int)COLOR.green ];
                    lineRenderer.material.color = Color.green;
                    if ( Input.GetMouseButtonDown( 0 ) ) {
                        cameraRig.transform.position = hitInfo.point + offset.normalized * viewDistance;
                    }   
                }
                sphere.GetComponent<Renderer>( ).material.color = color[ i ];
                lineRenderer.material.color = color[ i ];
                lineRenderer.SetPosition( i, linePoints[ i ].position );
                lineRenderer.SetWidth( 0.1f, 0.1f );
                Debug.DrawLine( linePoints[ (int)COLOR.red ].position, linePoints[ (int)COLOR.green ].position, color[ i ] );
            }
        }    
	}
}



