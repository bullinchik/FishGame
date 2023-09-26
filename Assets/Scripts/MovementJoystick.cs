using UnityEngine;
using UnityEngine.EventSystems;

public class MovementJoystick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Canvas _playerCanvas;
    [SerializeField] private GameObject tutor;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject upgrades;

    private Vector3 joystickVec;
    private Vector3 joystickTouchPos;
    private Vector3 playerStartPos;

    private bool _isJoystickActive;
    private float hWidth;
    private bool drag;
    private float delta;

    private void changeJoystickVisibility(bool visibility)
    {
        _isJoystickActive = visibility;
        tutor.SetActive(visibility);
        panel.SetActive(visibility);
        upgrades.SetActive(visibility);
    }

    private void Awake()
    {
        drag = false;
        _isJoystickActive = false;
        hWidth = Screen.width * 0.5f;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        changeJoystickVisibility(false);
        _isJoystickActive = true;
        joystickTouchPos = eventData.position;
        playerStartPos = _playerController.transform.position;
    }

    private void endDrag()
    {
        joystickVec = Vector3.zero;
        changeJoystickVisibility(false);
        drag = false;
    }

    private void FixedUpdate()
    {
        if (!EatingFood.finishing && drag)
        {
            float k = delta / hWidth * 5;

            Vector3 target = new Vector3(playerStartPos.x + k, _playerController.transform.position.y, _playerController.transform.position.z);

            float smoothSpeed = 0.1f;

            RaycastHit hit;
            if (Physics.Raycast(_playerController.transform.position, target - _playerController.transform.position, out hit, 1, 7))
            {
                return;
            }

            _playerController.transform.position = Vector3.Lerp(
                _playerController.transform.position, 
                target, 
                smoothSpeed);

            _playerCanvas.transform.position = Vector3.Lerp(
                _playerCanvas.transform.position, 
                new Vector3(target.x, _playerCanvas.transform.position.y, target.z), 
                smoothSpeed);
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        delta = eventData.position.x - joystickTouchPos.x;
        drag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endDrag();
    }

    public Vector2 GetJoystickDirection()
    {
        return joystickVec;
    }

    public bool IsJoystickPressed()
    {
        return _isJoystickActive;
    }
    
}