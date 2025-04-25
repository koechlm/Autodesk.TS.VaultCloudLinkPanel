Overview:
Cloud link panel is a simple extension that illustrates how to implement a custom panel hosting a web browser leveraging CefSharp libraries. 
 
It contains the following features/concepts:
	- Custom Dock Panel
		- embedded Chrome (CefSharp) browser control

To Use:
Open VaultCloudLinkExtension.sln in Visual Studio.  The project should open and compile with no errors.
Deploy the built files to %programData%\Autodesk\Vault 2026\Extensions\VaultCloudLinkExtension.
Run Vault Explorer.  There should be a "Vault Cloud Link Panel" you can dock or undock like the Property or Shared View panel.
If the panel does not appear, click View -> and enable the Vault Cloud Link Panel.

Known issues:
- There is almost no error-handling code.

DISCLAIMER:
---------------------------------
In any case, all binaries, configuration code, templates, and snippets of this solution are of "work in progress" character.
This also applies to GitHub "Release" versions.
Neither Markus Koechl nor Autodesk represents that these samples are reliable, accurate, complete, or otherwise valid. 
Accordingly, those configuration samples are provided as is with no warranty of any kind, and you use the applications at your own risk.

By downloading or copying parts of the solution, you confirm that you have read, understood, and accepted this disclaimer.

Sincerely,
Markus
