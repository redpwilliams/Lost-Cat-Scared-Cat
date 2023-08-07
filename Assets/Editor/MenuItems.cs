using UnityEditor;
using UnityEngine;

public class MenuItems : Editor
{
    private static Preferences _prefs;
    private const string CustomMenu = "Preferences/";
    private const string TutorialPath = CustomMenu + "Show Tutorial";
    private const string CatSelectionPath = CustomMenu + "Cat Selection/";

    private void OnEnable()
    {
        _prefs = SaveSystem.LoadPreferences();
        Menu.SetChecked($"{CatSelectionPath}Cat {_prefs.CatID}", true);
    }

    #region Tutorial
    
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

    #endregion

    #region Cat 1
    
    [MenuItem(CatSelectionPath + "Cat 1")]
    private static void SelectCat1()
    {
        _prefs.CatID = 1;
        SaveSystem.SavePreferences(_prefs);
    }

    [MenuItem(CatSelectionPath + "Cat 1", true)]
    private static bool SelectCat1Validate()
    {
        Menu.SetChecked(CatSelectionPath + "Cat 1", _prefs.CatID == 1);
        return true;
    }
    
    #endregion
    
    #region Cat 2
    
    [MenuItem(CatSelectionPath + "Cat 2")]
    private static void SelectCat2()
    {
        _prefs.CatID = 2;
        SaveSystem.SavePreferences(_prefs);
    }

    [MenuItem(CatSelectionPath + "Cat 2", true)]
    private static bool SelectCat2Validate()
    {
        Menu.SetChecked(CatSelectionPath + "Cat 2", _prefs.CatID == 2);
        return true;
    }
    
    #endregion
    
    #region Cat 3
    
    [MenuItem(CatSelectionPath + "Cat 3")]
    private static void SelectCat3()
    {
        _prefs.CatID = 3;
        SaveSystem.SavePreferences(_prefs);
    }

    [MenuItem(CatSelectionPath + "Cat 3", true)]
    private static bool SelectCat3Validate()
    {
        Menu.SetChecked(CatSelectionPath + "Cat 3", _prefs.CatID == 3);
        return true;
    }
    
    #endregion
    
    #region Cat 4
    
    [MenuItem(CatSelectionPath + "Cat 4")]
    private static void SelectCat4()
    {
        _prefs.CatID = 4;
        SaveSystem.SavePreferences(_prefs);
    }

    [MenuItem(CatSelectionPath + "Cat 4", true)]
    private static bool SelectCat4Validate()
    {
        Menu.SetChecked(CatSelectionPath + "Cat 4", _prefs.CatID == 4);
        return true;
    }
    
    #endregion
    
    #region Cat 5
    
    [MenuItem(CatSelectionPath + "Cat 5")]
    private static void SelectCat5()
    {
        _prefs.CatID = 5;
        SaveSystem.SavePreferences(_prefs);
    }

    [MenuItem(CatSelectionPath + "Cat 5", true)]
    private static bool SelectCat5Validate()
    {
        Menu.SetChecked(CatSelectionPath + "Cat 5", _prefs.CatID == 5);
        return true;
    }
    
    #endregion
    
}