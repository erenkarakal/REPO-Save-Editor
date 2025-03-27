# REPO-Save-Editor
CLI tool for encrypting and decrypting R.E.P.O. save files.
- Requires [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

# Usage
- [Download](https://github.com/erenkarakal/REPO-Save-Editor/releases/latest) the save editor.
- Open your save folder (inside `/AppData/LocalLow/semiwork`).
- Grab a save file (.es3 extension).
- Make a backup of it just in case.
- Drag it over the program, and it will be decrypted and saved as a .json file. You can edit the .json file with Notepad.
![decrypt](https://github.com/user-attachments/assets/635978fc-b75b-419c-a36c-db3b9c5a0c61)

- Once you are done editing, drag the .json file over the program again, and it will be encrypted and saved as an .es3 file.
![encrypt](https://github.com/user-attachments/assets/3f6c0ee3-0144-485c-a544-a8a2e51572b3)

# Info About Values

- Everything related to currencies is in thousands, so if the save file has "500", it is "500k" ingame.
- The `level` stat is 1 less than the ingame level; level 5 in the save file is level 6 ingame.
- Player properties like `playerHealth` and `playerUpgradeHealth`... use Steam IDs as keys.
- The `timePlayed` value is in seconds.
- Adding a new player works this way:
```
addPlayer(SteamID)
  set playerHealth for SteamID to 100
  add SteamID to playerNames value

  for every key starting with "player" (ex. playerUpgradeHealth)
    if the key doesnt contain SteamID, add it and set it to 0
```
