using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SM_CreateNewSave : MonoBehaviour
{
    public static string[] adjectiveList = new string[] { "Cool", "Precarious", "Extraordinary", "Spontaneous", "Suspicious", "Tremendous", "Overwhelming", "Underwhelming", "Caustic", "Radiant", "Disgusting", "Revolting", "Epic", "Crazy", "Insane", "Adventurous", "Expensive", "Strict", "Lazy", "Courageous", "Brilliant", "Silly", "Kind", "Outrageous", "Studious", "Dumb", "Particular", "Glamorous", "Reserved", "Optical", "Peaceful", "Relaxed", "Stoic", "Benevolent", "Elegant", "Knock-off", "Genuine", "Foreign", "Scared", "Terrible", "Mid"};
    public static string[] nounList = new string[] { "Cow", "Elephant", "Coffee", "Dog", "Cat", "Mug", "Car", "Subway", "Truck", "Garbage", "Bottle", "Pumpkin", "Chair", "Popcorn", "Table", "Guitar", "Drum", "Match", "Tea", "Tower", "Knife", "Horse", "Prop", "Acorn", "Walnut", "Book", "Lasagna", "Piano", "Broadcast", "Mochi", "Boba", "Octopus", "Whale", "Pencil", "Calendar", "Candle", "Chandelier", "Receipt", "Costume", "Hat", "Microwave", "Bus", "Train", "Line", "Lighter", "Cube", "Fart", "Doorknob", "Phone", "Light", "Lung"};

    public UIInput saveNameInput;
    public UIInput seedInput;
    public UIToggle isOnlineToggle;
    public UIInput onlinePasscode;

    private void OnEnable()
    {
        GenerateRandomName();
    }

    public void GenerateRandomName()
    {
        saveNameInput.SetText($"{adjectiveList[Random.Range(0, adjectiveList.Length)]} {nounList[Random.Range(0, adjectiveList.Length)]}");
    }

    public void Update()
    {
        onlinePasscode.gameObject.SetActive(isOnlineToggle.value);
    }

    public void Create()
    {
        Universe.isSetup = false;
        Fader.INSTANCE.FadeTo(new System.Action(StartNewUniverse), () => Universe.isSetup, 0.7f);
    }

    public void StartNewUniverse()
    {
        Universe.generateFromSave = null;
        Universe.universeGenPreset = "Default";
        Universe.universeName = saveNameInput.ReadText();
        Universe.universeSeed = seedInput.ReadText() == "" ? Extra.RandomSeed() : seedInput.ReadText();
        Universe.saveIsOnline = isOnlineToggle.value;
        Universe.savePasscode = onlinePasscode.ReadText();
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
