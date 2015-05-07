# Wexy-Backdoor 1.0
Wexy backdoor is a client/server C# application that I'm working on for fun. 
The server program must be ran on the victim's machine, and the hacker uses the client to connect to 
the server program which listens and executes commands sent from the client program.
It only works on a local network at the moment , as i'm using TcpClient / tcpListenner classes.

[HOW DOES IT WORK ?]
When the victim runs the Windows Defender executable, the server, it does three things : 
1 - It creates a new key labelled 'xFirewall' in the startup registry. 
2 - It starts as a hidden console application, but it is still shown in process list as 'Windows Defender.exe'
3 - It sends a mail to the attacker , that says 'Wexy is alive at [x.x.x.x] (the computer's local IP adress)
For configuring your mail, you must change the method 'AlertAttacker()' in server.cs class. 

[THE CLIENT PROGRAM] 
You execute the client and connects with the received IP adress. 
From here , you can do whatever the client allows you to do : 
![alt tag](http://oi61.tinypic.com/s0zxqc.jpg)

