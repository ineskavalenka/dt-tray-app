
I like to label things with a timestamp, but I'm too lazy to type the timestamp, so I created this app.  
This app sits in tray quietly, and puts the current timestamp to your clipboard whenever you need.   
With a single click.

Useful for:
 - creating dummy filenames that look random and non-distracting, but are sorted by timestamp
 - adding timestamp to notes and personal logs, e.g. meeting notes, ideas or diary entries

## Usage

Click on it's icon with the left mouse button, and the timestamp will be copied using the selected format.  

![](https://github.com/ineskavalenka/dt-tray-app/blob/main/img/tray-icon-click.png)  

Click on it with the right mouse button to open the timeformats menu and select other format.  
Selected format now becomes the default.
The current timestamp is also copied when you select it as default.  

![](https://github.com/ineskavalenka/dt-tray-app/blob/main/img/timeformats-context-menu.png)  

![](https://github.com/ineskavalenka/dt-tray-app/blob/main/img/usage.gif)  

## Configuring

To add or edit available timeformats:  
 - Open you app directory and find the `timeformats` file. Open it with notepad.
 - Write your formats one per line

Changes will take place on application restart.

![](https://github.com/ineskavalenka/dt-tray-app/blob/main/img/open-with-notepad.png)  

![](https://github.com/ineskavalenka/dt-tray-app/blob/main/img/editing-timeformats.png)  

Formats are used as follows:  
`Clipboard.SetText($"{DateTime.Now.ToString(format)}");`

where `format` is a line of `timeformats` file.

## Installation (Win10)

![dt-app.zip](https://github.com/ineskavalenka/dt-tray-app/blob/main/build)  

Put the program in "C:\Program Files\dt-app\"

`Win+R` -> `shell:startup` -> `C:\Users\..\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup`
![](https://github.com/ineskavalenka/dt-tray-app/blob/main/img/shell-startup.png)  

Add a shortcut to startup:
```
Start in: "C:\Program Files\dt-app\"
Target: "C:\Program Files\dt-app\dt-app.exe"
```

Now the app will start automatically on system startup.
