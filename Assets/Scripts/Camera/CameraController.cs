using UnityEngine;

public class CameraController : MonoBehaviour {
  public Camera Camera;
  public Transform followtransform;

  public float fastMovementSpeed;
  public float normalMovementSpeed;
  public float rotationSpeed;
  public float movementTime;
  // [Range(0.0f, 10.0f)]
  // public Vector2 minMaxZoom;

  private Vector3 newPosition;
  private Quaternion newRotation;
  private Vector3 newZoom;

  private float movementSpeed;
  private Vector3 camerLocalForward;

  private Plane plane; // For screen ray casts 
  private Vector3 dragStartPosition;
  private Vector3 dragCurrentPosition;
  private Vector3 rotationStartPosition;
  private Vector3 rotationCurrentPosition;

  void Start() {
    newPosition = transform.position;
    newRotation = transform.rotation;
    newZoom = Camera.transform.localPosition;
    camerLocalForward = Vector3.Normalize(Camera.transform.localPosition);
    plane = new Plane(Vector3.up, Vector3.zero);
  }

  void Update() {
    if (followtransform != null) {
      transform.position = followtransform.position;
      return;
    }
    HandleMouseInput();
    HandleMovementInput();

  }

  void HandleMouseInput() {
    // ---------------- Zoom ---------------------
    if (Input.mouseScrollDelta.y != 0) {
      newZoom += camerLocalForward * -Input.mouseScrollDelta.y * movementSpeed * 12;
    }
    //---------------- Movement ------------------
    if (Input.GetMouseButtonDown(0)) {
      Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
      if (plane.Raycast(ray, out float entry)) {
        dragStartPosition = ray.GetPoint(entry);
      }
    }
    if (Input.GetMouseButton(0)) {
      Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
      if (plane.Raycast(ray, out float entry)) {
        dragCurrentPosition = ray.GetPoint(entry);
        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
      }
    }
    if (Input.GetMouseButtonDown(2)) {
      rotationStartPosition = Input.mousePosition;
    }
    if (Input.GetMouseButton(2)) {
      rotationCurrentPosition = Input.mousePosition;
      Vector3 differnce = rotationCurrentPosition - rotationStartPosition;
      rotationStartPosition = Input.mousePosition;
      newRotation *= Quaternion.Euler(Vector3.up * (-differnce.x * 0.2f));
    }
  }

  void HandleMovementInput() {
    bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
    movementSpeed = isShiftPressed ? fastMovementSpeed : normalMovementSpeed;
    // --------------------- Movement --------------------
    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
      newPosition += transform.forward * movementSpeed;
    }
    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
      newPosition -= transform.forward * movementSpeed;
    }
    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
      newPosition += transform.right * movementSpeed;
    }
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
      newPosition -= transform.right * movementSpeed;
    }
    // --------------------- Rotation --------------------
    if (Input.GetKey(KeyCode.Q)) {
      newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
    }
    if (Input.GetKey(KeyCode.E)) {
      newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
    }
    // --------------------- Zoom --------------------
    if (Input.GetKey(KeyCode.R)) {
      newZoom += camerLocalForward * movementSpeed;
    }
    if (Input.GetKey(KeyCode.F)) {
      newZoom -= camerLocalForward * movementSpeed;
    }
    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    Camera.transform.localPosition = Vector3.Lerp(Camera.transform.localPosition, newZoom, Time.deltaTime * movementTime);
  }
}
