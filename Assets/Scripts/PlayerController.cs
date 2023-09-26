using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MovementJoystick joystick;
    public UserResources ur;
    public GameObject model;

    #region private variables
    private Rigidbody _rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Canvas _playerCanvas;

    private float mainCameraPositionX;
    private Vector3 currentVelocity;

    #endregion
    void Start()
    {
        ur.UpdateMesh();
        model = Instantiate(UserResources.model, this.transform);
        //GetComponent<MeshFilter>().mesh = UserResources.mesh;
       // GetComponent<MeshRenderer>().materials = UserResources.stageMaterials;
        _rb = GetComponent<Rigidbody>();
        currentVelocity = Vector3.zero;

        mainCameraPositionX = Camera.main.transform.position.x;

        Camera.main.transform.position = new Vector3(mainCameraPositionX + _rb.position.x, 4f, _rb.position.z - 5f);
    }

    private void FixedUpdate()
    {
        if (joystick.IsJoystickPressed() || EatingFood.finishing)
        {
            MoveForward();
        }
        
        if (EatingFood.finishing)
        {
            speed = 10;
        }
    }

    public void MoveForward()
    {
        Vector3 movement = new Vector3(0f, 0f, 1f * speed * Time.deltaTime);
        float xCameraPosition = Camera.main.transform.position.x;


        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, new Vector3( mainCameraPositionX + _rb.position.x, 4f, _rb.position.z - 4f), ref currentVelocity, 0.1f);
        transform.Translate(movement, Space.World);
        _playerCanvas.transform.Translate(movement, Space.World);
    }
}
