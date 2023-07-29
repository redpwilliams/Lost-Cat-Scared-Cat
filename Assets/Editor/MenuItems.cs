using UnityEditor;
using UnityEngine;

public class MenuItems : Editor
{
    private static Preferences _prefs;
    private const string CustomMenu = "Preferences/";
    private const string TutorialPath = CustomMenu + "IsFirstTime";

    private void OnEnable()
    {
        _prefs = SaveSystem.LoadPreferences();
    }

    [MenuItem(TutorialPath)]
    private static void IsFirstTime()
    {
        _prefs.IsFirstTime ^= true;
        SaveSystem.SavePreferences(_prefs);
        Debug.Log(_prefs);

    }

    [MenuItem(TutorialPath, true)]
    private static bool IsFirstTimeValidate()
    {
        Menu.SetChecked(TutorialPath, _prefs.IsFirstTime);
        return true;
    }
}
