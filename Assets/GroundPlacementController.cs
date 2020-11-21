using UnityEngine;
using DG.Tweening;

public class GroundPlacementController : MonoBehaviour {
  [SerializeField]
  private GameObject placeableObjectPrefab;

  [SerializeField]
  private float rotationSpeed;
  [SerializeField]
  private float rotationTime;

  [SerializeField]
  private MeshCollider groundCollider;

  [SerializeField]
  private KeyCode newObjectHotkey = KeyCode.A;

  private GameObject currentPlaceableObject;
  private Quaternion newRotation;


  private void Update() {
    HandleNewObjectHotkey();

    if (currentPlaceableObject != null) {
      MoveCurrentObjectToMouse();
      RotateFromMouseWheel();
      ReleaseIfClicked();
    }
  }

  private void HandleNewObjectHotkey() {
    if (Input.GetKeyDown(newObjectHotkey)) {
      if (currentPlaceableObject != null) {
        Destroy(currentPlaceableObject);
      } else {
        currentPlaceableObject = Instantiate(placeableObjectPrefab);
        newRotation = currentPlaceableObject.transform.rotation;
      }
    }
  }

  private void MoveCurrentObjectToMouse() {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    RaycastHit hitInfo;
    if (groundCollider.Raycast(ray, out hitInfo, 100f)) {
      currentPlaceableObject.transform.position = hitInfo.point;
      // currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
    }
  }

  private void RotateFromMouseWheel() {
    if (Input.GetAxis("Mouse ScrollWheel") != 0) {
      float direction = Input.GetAxis("Mouse ScrollWheel") > 0 ? 1f : -1f;
      newRotation *= Quaternion.Euler(Vector3.up * direction * rotationSpeed);
      currentPlaceableObject.transform.DORotateQuaternion(newRotation, rotationTime).SetEase(Ease.OutBounce, 1.1f); // = Quaternion.Lerp(currentPlaceableObject.transform.rotation, newRotation, Time.deltaTime * rotationTime);
    }
  }

  private void ReleaseIfClicked() {
    if (Input.GetMouseButtonDown(0)) {
      currentPlaceableObject = null;
    }
  }
}