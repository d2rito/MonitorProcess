How to try the monitoring utility:

1. Open a Command Prompt. You can do this by pressing the windows key in your keyboard an typing 'cmd'
2. Go to your diretory thaat has the .exe of the app. To do this you have to locate wherever you unzipped the zip, and then type in the cmd "cd ~\MonitorProcess\bin\Debug\net6.0\" (In the ~ goes your directory)
3. Once you are in the correct directory try passing the arguments corresponding your process you want to monitor, an example can be "notepad" you can run the following command line: "MonitorProcess.exe notepad 5 1"
4. The program should be starting monitoring the specified process based on the line arguments.