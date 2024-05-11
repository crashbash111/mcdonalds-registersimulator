# mcdonalds-registersimulator
A NP6 Register Simulator for training purposes only.

![image](https://github.com/crashbash111/mcdonalds-registersimulator/assets/50429378/6e84b07d-7cfa-4213-90dd-412096068ddd)


## About this project
This project was created for training purposes to provide a way for employees to practice using an NewPos6 system without any impact to a running store environment. It is written in C# and is designed to read files from a NewPos6 FC POS register and simulate the User Interface to help employees become farmilar with the layout, before they are expected to use real in-store equipment.
As this project can dynamically read in register files, it will ultimately pull the latest copy from the local waystation if running on the local network. If the waystation cannot be found, it will prompt the user to provide the files themselves. Whenever there is a new update to all POS, the simulator should be able to pull a new local copy at any time.

![image](https://github.com/crashbash111/mcdonalds-registersimulator/assets/50429378/c7328b0b-d704-469e-96fb-c55980b0d1ef)

## Legal Disclaimer

This repository contains reverse-engineered software intended for educational and research purposes only. The "McDonald's" name, logo, and other related trademarks are owned by McDonald's Corporation and are used here for informational purposes only.

The code in this repository is developed independently and does not contain any proprietary code, software, or intellectual property owned by McDonald's Corporation. This project does not include any content from McDonald’s official sources, and all implementations are created by the repository contributors based on publicly available information or self-generated data.

Users of this repository are required to obtain any necessary software or data from legitimate sources, and must comply with all applicable laws and regulations, including those concerning copyrights and trademarks. The creator of this repository does not assume any responsibility for unauthorized use of this content.

Please ensure that your use of this material complies with all legal and ethical standards applicable in your jurisdiction.
In order to run this application, you must already have access to an existing store installation and obtain the files on your own.

The contents of the Posdata you need to provide look a bit like this:

![image](https://github.com/crashbash111/mcdonalds-registersimulator/assets/50429378/8039989a-6d26-4dc3-9048-636433143916)
