using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject menu;
    public GameObject instruction;
    public GameObject story;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCredits()
    {
        
        menu.SetActive(false); 
        creditsPanel.SetActive(true); 
    }

    // Method to close the credits panel
    public void CloseCredits()
    {
        creditsPanel.SetActive(false); 
        menu.SetActive(true); 
    }

    public void OpenInstructions()
    {

        menu.SetActive(false);
        instruction.SetActive(true);
    }

    // Method to close the credits panel
    public void CloseInstructions()
    {
        instruction.SetActive(false);
        menu.SetActive(true);
    }

    public void OpenStory()
    {

        menu.SetActive(false);
        story.SetActive(true);
    }

    // Method to close the credits panel
    public void CloseStory()
    {
        story.SetActive(false);
        menu.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

}
