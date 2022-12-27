# InputCounter

This is a small program which counts the keyboard strokes and the mouse clicks (left and right button).

## Where is the data stored (aka Security / Keyboard-Sniffer)

The data is stored locally in an SQLite database (`InputCountDatabase.db`). The data is only used locally on the computer and is not transferred.

If you want to look at the data, you can use the free program [DB Browser](https://sqlitebrowser.org).

## How does it work?

The tool uses a low level keyboard and mouse hook to detect an action. So it can maybe possible that your anti virus tool will alert you.

I use Windows Defender and have had no problems so far
