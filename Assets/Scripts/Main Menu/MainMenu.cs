using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main_Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Animator image;
    
        public void StartGame()
        {
            image.Play("FadeOut", -1, 0f);
            StartCoroutine(OpenFirstLevel());
        }

        private static IEnumerator OpenFirstLevel()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }
    }
}
