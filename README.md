# Wexy-Backdoor 1.0
Wexy backdoor is a client/server C# application that I'm working on for fun. 
The server program must be ran on the victim's machine, and the hacker uses the client to connect to 
the server program which listens and executes commands sent from the client program.
It only works on a local network at the moment , as i'm using TcpClient / tcpListenner classes.

[HOW DOES IT WORK ?]
When the victim runs the Windows Defender executable, the server, it does three things : <br />
1 - It creates a new key labelled 'xFirewall' in the startup registry. <br />
2 - It starts as a hidden console application, but it is still shown in process list as 'Windows Defender.exe'<br />
3 - It sends a mail to the attacker , that says 'Wexy is alive at x.x.x.x (the computer's local IP adress)<br />
For configuring your mail, you must change the method 'AlertAttacker()' in server.cs class. 
<br /><br />
I am currently trying to add a keylogger and I'm working on a fork of it , allowing the backdoor to be used outside of the local network by using OPEN.NAT framework to open ports and UDP data transfer.
<br />
If you wish to keep working on it , you need to download the krypton toolkit , I used it for the GUI
https://www.componentsource.com/product/krypton-toolkit/downloads

[THE CLIENT PROGRAM] 
You execute the client and connects with the received IP adress. 
From here , you can do whatever the client allows you to do : 
- See the OS version 
- See the Computer name 
- Open / Delete / download a file directly from the remote computer(download needs to be fixed though)
- Open a website on the remote computer with internet explorer(because it's a vulnerable browser)
- send a file via mail(the mail of the sender and its password and the mail receiver)
- Get the login data file that contains the google chrome passwords
- send a message to the remote computer
- explore files and folders on the remote computer
- make the remote computer download a file(blue button on the right)
- take a screenshot of the remote computer's screen (black button on the right)
- shut down the backdoor if you're tired of hacking the victim lol
![alt tag](http://s10.postimg.org/8vzysychl/2015_07_27_125612.png)

