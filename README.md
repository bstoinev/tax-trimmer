# Tax Trimmer
This project is part of the HR process in DevOcean

No exception is thrown if the grossAmount is a negative number. However, the net amount defaults to 0.

# Usage
- Fork/clone the repo.
- Hit F5 or build and launch TaxTrim.exe (The DevOcean.TaxTrim.Cli assembly)

The application will enter an input mode asking you to enter a gross amount. Enter a positive, decimal number and hit Enter. 
The net amount will be displayed on the next line.
Repeat the procedure as needed.

To return to the Windows Command Prompt press Ctrl+C.

# Configuration

All the settings of the tax system are configurable through the appsettings.json file 

# Motivation

Why is the application developed the way it is? 

1. Over-engineering
For the sake of the test, the app utilizies DI, Logging, error handling, and other good practices to merely demonstrate their usage.

2. Enforce loose coupling
	Current assembly structure is as follows: 
		Application code: DevOcean.TaxTrimmer.dll
		Application code tests: DevOcean.TaxTrimmer.Xunit.dll
		Hosting app: DevOcean.TaxTrimmer.Cli.exe

	Application code is located in a separate assembly so it coud be plugged in whatever other host app is necessary. 
	E.g., Windows Service, ASP.NET Web App, etc.
	
3. Logging
	It is merely a demonstration of the 1st OOP principle: Abstraction
	Besides, there are too many log providers nowadays so hard coupling some in the application is a limitting experience. 
	
	
Perhaps you have more questions about the coding? Shoot!
