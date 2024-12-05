; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "R8 Control"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "Gil Yoder"
#define MyAppURL "https://github.com/jgyo/r8-control"
#define MyAppExeName "R8LocoCtrl.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{73F89AB2-6AF8-4349-881F-1EFE2FED9B76}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName=c:\{#MyAppName}
; "ArchitecturesAllowed=x64compatible" specifies that Setup cannot run
; on anything but x64 and Windows 11 on Arm.
ArchitecturesAllowed=x64compatible
; "ArchitecturesInstallIn64BitMode=x64compatible" requests that the
; install be done in "64-bit mode" on x64 or Windows 11 on Arm,
; meaning it should use the native 64-bit Program Files directory and
; the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64compatible
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=..\LICENSE
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
SetupIconFile=.\icons\R8Control.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: ".\bin\Release\net8.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\net8.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
const
  DotnetRequiredMsg = 'DotNet 8.0 or greater is required, but not installed. Do you want to download DotNet now?';
  DotnetUrlMsg = 'https://dotnet.microsoft.com/en-us/download/dotnet';
  UrlErrorMsg = 'Unable to open the Dotnet download page. Please search for Dotnet on the web, and download 8.0 or later manually.';
  RestartMsg = 'Please restart this setup after installing Dotnet 8.0 or above. Thank you!';

function DetectAndInstallPrerequisites: Boolean;
var
  dialogResult: Integer;
  ResultCode: Integer;
  
begin
  (*** Place your prerequisite detection and extraction+installation code below. ***)
  (*** Return False if missing prerequisites were detected but their installation failed, else return True. ***)

  ExtractTemporaryFile('CheckDotNetVersion.exe');
  if Exec(ExpandConstant('{tmp}\CheckDotNetVersion.exe'), '8.0', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode) then
  begin
    // Success! Nothing to do.
    if ResultCode = 0 then
    begin
      Result := True;
    end
    else begin
      // Failure! Tell user
      dialogResult := MsgBox(DotnetRequiredMsg, mbInformation, MB_YESNO);
      if dialogResult = IDYES then
      begin
        // function Exec(const Filename, Params, WorkingDir: String; const ShowCmd: Integer; const Wait: TExecWait; var ResultCode: Integer): Boolean;
        // function ShellExec(const Verb, Filename, Params, WorkingDir: String; const ShowCmd: Integer; const Wait: TExecWait; var ErrorCode: Integer): Boolean;
        if not ShellExec('', DotnetUrlMsg, '', '', SW_SHOWNORMAL, ewNoWait, ResultCode) then
        begin
          MsgBox(UrlErrorMsg, mbError, MB_OK);
        end
      end;-
      Result := False;
    end;
  end;
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  if DetectAndInstallPrerequisites then
  begin
    NeedsRestart := False;
    Result := '';
  end else
    Result := RestartMsg;  
end;