﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Animator animator;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = string.Empty;
        int baliseIndex = -1;
        string preBalise = "";
        string postBalise = "";
        foreach (char letter in sentence.ToCharArray())
        {
            // new balise
            if(letter == '<')
            {
                if (preBalise == string.Empty)
                {
                    baliseIndex = dialogueText.text.Length - 1;
                    preBalise += letter;
                }
                else
                {
                    postBalise += letter;
                }
            }
            // End of balise
            else if(letter == '>' && postBalise != "")
            {
                postBalise += letter;
                dialogueText.text = dialogueText.text.Insert(baliseIndex, preBalise);
                dialogueText.text += postBalise;
                baliseIndex = -1;
                preBalise = "";
                postBalise = "";
            }
            else
            {
                if(preBalise != "" && preBalise.Last() != '>')
                {
                    preBalise += letter;
                }
                else if (postBalise != "" && postBalise.Last() != '>')
                {
                    postBalise += letter;
                }
                else
                {
                    dialogueText.text += letter;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

}
