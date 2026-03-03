[Setup]
AppName=KMCPrescription
AppVersion=1.0.0
WizardStyle=modern
DefaultDirName={autopf}\KMCPrescription
DefaultGroupName=KMCPrescription
Compression=lzma2
SolidCompression=yes
OutputDir=C:\Users\Administrator\Desktop\KMCPrescription
OutputBaseFilename=KMCPrescription
SetupIconFile=C:\Users\Administrator\Documents\GitHub\kmc_prescription\KMCPrescriptiom\bin\Release\logo.ico

ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=

[Files]
Source: "C:\Users\Administrator\Documents\GitHub\kmc_prescription\KMCPrescriptiom\bin\Release\*"; \
DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Note: log4net.dll will be automatically included via recursesubdirs
; The post-build script ensures the correct version (1.2.10.0) is copied to Release folder

[Icons]
Name: "{group}\KMC Prescription App"; Filename: "{app}\KMCPrescriptiom.exe"
Name: "{commondesktop}\KMC Prescription App"; Filename: "{app}\KMCPrescriptiom.exe"

[Run]
Filename: "{app}\KMCPrescriptiom.exe"; \
Description: "Launch KMC Prescription App"; Flags: nowait postinstall skipifsilent
