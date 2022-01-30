using System;
using System.Collections;
using Button_Affected_Objects;
using Buttons;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private bool retainOnLeave = true;
    [SerializeField] private Animator image;
    
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("White Button"))
            collision.transform.GetComponent<ButtonController>().Trigger();
        if (collision.transform.CompareTag("Black Button"))
            collision.transform.GetComponent<ButtonController>().Trigger();
    }

    private static void Trigger(Component other, int action = 0)
    {
        var components = other.GetComponents(typeof(ButtonController));

        foreach (var component in components)
        {
            var buttonController = (ButtonController) component;
            if (action == 0)
                buttonController.Trigger();
            else
                buttonController.ResetPosition();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("White Button"))
            Trigger(other);
        else if (other.CompareTag("Next Level"))
        {
            image.Play("FadeOut", -1, 0f);
            StartCoroutine(Delay());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Reset"))
            Trigger(other, 1);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("White Button") || !retainOnLeave) return;
        Trigger(other);
    }
}
