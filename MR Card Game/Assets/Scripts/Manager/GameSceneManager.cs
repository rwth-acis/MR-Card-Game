using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{

    [Tooltip("Define the game object under which all models are saved (so that they can be found with their name, even if they have the same name as a game object")]
    public GameObject saveModelObject;

    [SerializeField] private GameObject planeFinder;

    [SerializeField] private TextMeshProUGUI toggleGameboardButtonText;

    private bool firstImport = true;

    private bool gameBoardLocked = false;

    private void Awake()
    {
        ImportAllModels();
    }

    public void LoadIntroSceneAsync()
    {
        SceneManager.sceneLoaded -= ActiveIntroSceneOnLoad;
        SceneManager.sceneLoaded += ActiveIntroSceneOnLoad;
        SceneManager.UnloadSceneAsync("GameScene");
        SceneManager.LoadSceneAsync("IntroScene", LoadSceneMode.Additive);

    }

    private void LockGameboard()
    {
        gameBoardLocked = true;
        planeFinder.SetActive(false);
        toggleGameboardButtonText.text = "Unlock Gameboard";
    }

    private void UnlockGameboard()
    {
        gameBoardLocked = false;
        planeFinder.SetActive(true);
        toggleGameboardButtonText.text = "Lock Gameboard";
    }

    public void ToggleGameboard()
    {
        if (gameBoardLocked)
        {
            UnlockGameboard();
        }
        else
        {
            LockGameboard();
        }
    }

    private void ActiveIntroSceneOnLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene == SceneManager.GetSceneByName("IntroScene"))
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    /// <summary>
    /// Method that imports all models, and sets them invisible. Is done at the begining of a round so that no wait time is needed while playing.
    /// </summary>
    public async void ImportAllModels()
    {
        // Check if this is the first time something is imported, and if it is needed to initialize the object importer
        if (firstImport == true)
        {
            // Initialize the object importer
            ObjImporter objImporter = new ObjImporter();
            if(!ServiceManager.ServiceExists<ObjImporter>())
            {
                ServiceManager.RegisterService(objImporter);
            }

            // Set the flag that states that this is the first import to false
            firstImport = false;
        }

        // Get the array of models
        string[] models = Directory.GetFiles(Questions.pathToLevel, "Model*", SearchOption.TopDirectoryOnly);

        // Set the number of models
        Questions.numberOfModels = models.Length;

        // Import all models
        foreach (string model in models)
        {
            // Access the model gameobject
            string json = File.ReadAllText(model);

            // Extract the gameobject
            Model modelObject = JsonUtility.FromJson<Model>(json);

            // Import the first model
            string url = modelObject.modelUrl;
            GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync(url);

            // Rename the object in the model object name
            obj.name = "Model_" + modelObject.modelName.Substring(0, modelObject.modelName.Length - 4);

            // Add the model tag
            obj.tag = "Model";

            Debug.Log("Model renamed in: " + "Model_" + modelObject.modelName.Substring(0, modelObject.modelName.Length - 4));

            // Initialize the model (resize it correctly) and set it as the child of the same model object
            InitializeModel(obj, saveModelObject);

            // When the model finished loading, increase the loaded model counter
            Questions.numberOfModelsLoaded = Questions.numberOfModelsLoaded + 1;

            Debug.Log("The current number of models that are loaded is: " + Questions.numberOfModelsLoaded);
            Debug.Log("Currently we have: " + Questions.numberOfModelsLoaded + " >= " + Questions.numberOfModels);
        }

        Debug.Log("The number of models is: " + Questions.numberOfModels);

        // Activate the game menu, where wave and currency are displayed
        ActivateGame();

        Board.activateGameBoard = false;
    }

    // Method that initializes the model, creates a box collider, resizes it, and sets it to the right position, sets the model as child of the parent
    private void InitializeModel(GameObject obj, GameObject parent)
    {
        // Get the child gamobject (there should always be one)
        // TODO check the number of children, and add one if needed
        GameObject childGameObject1 = obj.transform.GetChild(0).gameObject;
        // GameObject childGameObject1 = obj.transform.gameObject;

        // Add a box collider to the child
        childGameObject1.AddComponent<BoxCollider>();

        // Access the box collider information
        BoxCollider m_Collider = childGameObject1.GetComponent<BoxCollider>();

        childGameObject1.gameObject.layer = 10;

        // Get the greatest size of the sizes of the box collider
        float greatest = ReturnGreatestFloat(m_Collider.size.x, m_Collider.size.y, m_Collider.size.z);

        // Get the down scale factor you want
        float scale = (float)0.1 / greatest;

        // Down scale the model
        obj.transform.localScale = new Vector3(scale * 0.3f, scale * 0.3f, scale * 0.3f);

        // Set the model as child of the marker
        obj.transform.parent = parent.transform;
    }

    // Method that returns the greatest number of the three floats given
    private float ReturnGreatestFloat(float size1, float size2, float size3)
    {
        // Initialize a size variable
        float greater = 0;

        // Check which one is greater from the two first sizes
        if (size1 >= size2)
        {
            greater = size1;
        }
        else
        {
            greater = size2;
        }

        // Check what is greater, the greatest of the two first sizes or the third size
        if (greater >= size3)
        {
            return greater;
        }
        else
        {
            return size3;
        }
    }

    // Method that activates the view model menu and enables the user to open the first question
    private void ActivateGame()
    {

        // Make sure all towers are released
        // Get the array of all tower objects
        GameObject[] towerArray = GameObject.FindGameObjectsWithTag("Tower");

        // Disable all towers
        foreach (GameObject tower in towerArray)
        {
            // Check if the tower is active
            if (tower.activeSelf == true)
            {
                // Release the tower object
                ObjectPools.ReleaseTower(tower);
            }
        }

        // Get the array of all trap objects
        GameObject[] trapArray = GameObject.FindGameObjectsWithTag("Trap");

        // Disable all traps
        foreach (GameObject trap in trapArray)
        {
            // Check if the trap is active
            if (trap.activeSelf == true)
            {
                // Release the trap object
                ObjectPools.ReleaseTrap(trap.GetComponent<Trap>());
            }
        }

        // Reset all spell cards so that they are not drawn
        GameObject[] spellArray = GameObject.FindGameObjectsWithTag("Spell Card");

        foreach (GameObject spellCard in spellArray)
        {
            spellCard.GetComponent<SpellCard>().ResetSpellCard();
        }
    }
}
/// <summary>
/// The JSON Serialization for the Models
/// </summary>
[Serializable]
public class Model
{
    public string modelName;
    public string modelUrl;
    public int numberOfQuestionsUsedIn;
}
