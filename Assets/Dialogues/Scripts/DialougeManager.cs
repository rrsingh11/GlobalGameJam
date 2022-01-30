using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using  UnityEngine.UI;

public class DialougeManager : MonoBehaviour
{
    [SerializeField] private Animator image;

    public Text nameText;
    public Text dialogueText;
    public Text flipNameText;
    public Animator animator;
    
    private Queue<string> sentences;
    private Queue<string> names;
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();

    }

    public void StartDialouge(Dialouge dialouge)
    {
        animator.SetBool("IsOpen",true);
        Debug.Log($"Starting Conversation with {dialouge.name}");
        nameText.text = dialouge.name;

        sentences.Clear();
        names.Clear();
        
        foreach (string name1 in dialouge.names)
        {
            names.Enqueue(name1);
        }
        
        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialouge();
                return;
            }
            
            string name1 = names.Dequeue();
            StopAllCoroutines();
            StartCoroutine (TypeName(name1));
            string sentence = sentences.Dequeue();
            //StopAllCoroutines();
            StartCoroutine (TypeSentence(sentence));
        }
    
    IEnumerator TypeName(string name1)
    {
        flipNameText.text = "";
        foreach (char letter in name1.ToCharArray())
        {
            flipNameText.text+= letter;
            yield return null;
        }

    }
    
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text+= letter;
                yield return null;
            }

    }

        void EndDialouge()
        {
            animator.SetBool("IsOpen",false);
            Debug.Log($"End of Conversation");
            enabled = false;
            if (SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 5)
            {
                image.Play("FadeOut", -1, 0f);
                StartCoroutine(ChangeScene());
            }
        }

        private IEnumerator ChangeScene()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


