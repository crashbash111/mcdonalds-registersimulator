# mcdonalds-registersimulator
A NP6 Register Simulator for training purposes only.

![image](https://github.com/crashbash111/mcdonalds-registersimulator/assets/50429378/6e84b07d-7cfa-4213-90dd-412096068ddd)


## About this project
This project was created for training purposes to provide a way for employees to practice using an NewPos6 system without any impact to a running store environment. It is written in C# and is designed to read files from a NewPos6 FC POS register and simulate the User Interface to help employees become farmilar with the layout, before they are expected to use real in-store equipment.
As this project can dynamically read in register files, it will ultimately pull the latest copy from the local waystation if running on the local network. If the waystation cannot be found, it will prompt the user to provide the files themselves. Whenever there is a new update to all POS, the simulator should be able to pull a new local copy at any time.

![image](https://github.com/crashbash111/mcdonalds-registersimulator/assets/50429378/c7328b0b-d704-469e-96fb-c55980b0d1ef)


## Disclaimer
No proprietary code has been used in this project. This simulator reverse-engineers a running NP6 system. All code has been written from scratch.
As I do not have the appropriate licencing, no proprietary code has been included in this repository. In order to run this application, you must already have access to an existing store installation and obtain the files on your own.

The contents of the Posdata you need to provide look a bit like this:

![image](https://github.com/crashbash111/mcdonalds-registersimulator/assets/50429378/8039989a-6d26-4dc3-9048-636433143916)
