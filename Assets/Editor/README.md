# Using the Preferences Window

<img src="../Resources/PreferencesReference.png" />

The `Preferences` menu is meant to select player preferences in the development environment - instead of through the game. Ideally, this makes it easier to test how preferences affect game state without having to toggle the state through game.

- This actual menu is handled in [`MenuItems.cs`](./MenuItems.cs)
- The save system is handled with both [`Preferences.cs`](../Scripts/Settings/Preferences.cs) and [`SaveSystem.cs`](../Scripts/Settings/SaveSystem.cs)
