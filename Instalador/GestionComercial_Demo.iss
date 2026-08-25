; ==============================================================
;  GestionComercial — Inno Setup Script (Demo)
;  Genera instalador .exe con acceso directo y credenciales
; ==============================================================

#define MyAppName "GestionComercial"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "GestionComercial"
#define MyAppExeName "GestionComercial.UI.exe"

[Setup]
AppId={{B5E3A4D1-7C8F-4E2A-9D6B-1F3E5A8C2B4D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=Instalador\Output
OutputBaseFilename=GestionComercial_Demo_v{#MyAppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
DisableDirPage=no
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
SetupIconFile=logo.ico
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; DB pre-cargada: solo copiar si no existe (no sobrescribir datos del usuario)
Source: "C:\GestionComercial_Demo\GestionComercial.db"; DestDir: "{app}"; Flags: onlyifdoesntexist
; Copiar resto de archivos (exe, dlls, etc.)
Source: "C:\GestionComercial_Demo\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "GestionComercial.db"

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\{#MyAppExeName}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Iniciar {#MyAppName}"; Flags: nowait postinstall skipifsilent
