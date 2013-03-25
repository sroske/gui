########  What is this ReadMe about  ########

This readme is to get anyone just navigating to this folder for the first time familiar with the entire GUI project. This includes how it works and what you need to do to set it up.


###  Setting Up Your Desktop/Development Environment  ###

This project is being written in C# using Microsoft Visual C# Express 2010, which is available to download for free. Since Microsoft likes to make it difficult to not use the latest-and-"greatest" 2012 version, I've included the vca_web executable that will handle the downloading and installing for you. If for some reason this doesn't work, TELL ME IMMEDIATELY. This needs to work for anyone who wants to open this project, so I'll need to find a new way to let people download and install VS C# 2010 easily.

I have no idea how compatible this code (and the classes I use in the code to do various things) is with versions of Windows that aren't 7, so you're on your own as far as getting it to work on Windows XP, Windows 8, etc. More than likely, I would assume it will work on 8, but I don't know for sure. It's probably about 25% likely that it'll work on XP.

Once you have VS C# 2010 installed, you'll need to install some sort of serial-port emulator. I have included the necessary files to install com0com, which is the one I use. You're free to use whatever one you want, so make sure you can create two virtual COM ports and can connect them using the software.

Once that's installed, if you get some pop-up from Windows saying you need to use signed drivers, that means you'll need to turn test mode on before you can do anything with the GUI. (If you don't, ignore the rest of this section.) Run the "Toggle Signed Drivers" file, wait for it to say it's done, close it, and then restart your computer. You should then see a little watermark in the lower-right-hand corner telling you Windows is in Test Mode. If you do, great! You can move on. If you don't, something is amiss, and I have no clue what. So good luck figuring it out :)

Now that you're in test mode, you should be able to set up your virtual COM ports with com0com (or whatever you're using). In the code for the GUI, I have it so that it looks for ports named "COM5" and "COM6". Name your new virtual ports those and make sure they're connected. (If they aren't, then data won't be sent from one to the other correctly and the GUI won't do anything.) If for some reason you can't name your ports "COM5" and "COM6", then go into Program.cs in VS C# 2010 and change the instances of "COM5" and "COM6" to your existing virtual COM ports.

Now that' your ports are set up, the GUI can now do stuff. You should be able to run the application and get to the main window without any error messages.

IF YOU GET ANY ERROR MESSAGES OF ANY KIND, LET ME KNOW. Especially ones that mention .dll files. Those are incredibly important.