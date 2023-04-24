# FoldersSync
A C# program that syncs two folders.
The program syncs two folders periodically given a specified time interval and logs the actions onto the console and into a specified log file. The sync is done by overwritting previously existing files or deleting the files that no longer exist in the source folder.

To execute the code publish it via Visual Studio to a local folder then run it through a command line instance with the parameters:
start FoldersSync.exe [sourceFolder] [destinationFolder] [timeInterval] [logFilePath].
You can alternatively run it in Visual Studio by uncommenting the fixed values for the parametres and replacing them with your local paths that you want to test.
