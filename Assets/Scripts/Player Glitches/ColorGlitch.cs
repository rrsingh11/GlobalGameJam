using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player_Glitches
{
    public class ColorGlitch : MonoBehaviour
    {
        [SerializeField] private PlayerMeshGenerator playerMeshGenerator;
        [SerializeField] private MeshRenderer mesh;
        [SerializeField] private float timeToStartRotating;
        
        
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private PhysicMaterial physicMaterial;
        [SerializeField] private Transform slope;
        [SerializeField] private float rotationFactor = 1000f;
        [SerializeField] private Animator background;
        
        [SerializeField] private Material white, black;

        private const float Duration = 1f;

        private float _startTime;

        private bool startRotating;

        private bool _change;

        private void Update()
        {
            if (!_change) return;
            var t = (Time.time - _startTime) / Duration;
            playerMeshGenerator.sides = (int) Mathf.SmoothStep(4, 30, t);
            if (startRotating)
                slope.Rotate(0f, 0f, -rotationFactor * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Trigger")) return;
            _change = true;
            _startTime = Time.time;
            transform.GetChild(0).GetChild(0).gameObject.AddComponent<CapsuleCollider>();
            transform.GetChild(0).GetChild(0).parent = null;
            mesh.GetComponent<Rigidbody>().useGravity = true;
            transform.GetComponent<BoxCollider>().enabled = false;
            mesh.GetComponent<Rigidbody>().isKinematic = false;
            mesh.GetComponent<Rigidbody>().AddForce(new Vector3(10f, 0f, 0f));
            mesh.GetComponent<CapsuleCollider>().material = physicMaterial;
            cinemachineVirtualCamera.Follow = mesh.transform;
            StartCoroutine(ChangeBackgroundColor());
            StartCoroutine(ResetCameraDamping());
        }
        
        private IEnumerator ChangeBackgroundColor()
        {
            yield return new WaitForSeconds(6f);
            background.Play("Fade To Black BG", -1, 0f);
            startRotating = true;
            if (Camera.main != null)
                Camera.main.transform.GetChild(0).GetComponent<Animator>().Play("Fade To Black", -1, 0f);
        }

        private IEnumerator ResetCameraDamping()
        {
            yield return new WaitForSeconds(4f);
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0f;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0f;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0f;
        }
    }
}
