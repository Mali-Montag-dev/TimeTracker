; ==========================================================
;  TimeTracker - סקריפט התקנה (Inno Setup 6)
; ==========================================================

#define MyAppName "TimeTracker"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "Mali Montag"
#define MyAppURL "https://github.com/Mali-Montag-dev/TimeTracker"
#define MyAppExeName "TimeTracker.Desktop.exe"

[Setup]
; מזהה ייחודי של האפליקציה - לא לשנות אחרי שחרור ראשון
AppId={{D6036AA7-2DA3-4B64-9B8D-47FC838030DE}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}

; התקנה לתיקיית המשתמש - לא דורש הרשאות מנהל
DefaultDirName={localappdata}\Programs\{#MyAppName}
PrivilegesRequired=lowest
DisableProgramGroupPage=yes

; קובץ ההתקנה שייווצר
OutputDir=Output
OutputBaseFilename=TimeTracker-Setup-{#MyAppVersion}
SetupIconFile=..\TimeTracker.Desktop\Assets\app.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName}

; מראה ודחיסה
WizardStyle=modern
Compression=lzma2
SolidCompression=yes

; 64 ביט בלבד
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "startup"; Description: "הפעלה אוטומטית עם Windows"; GroupDescription: "אפשרויות נוספות:"

[Files]
; SQL Server LocalDB - מועתק לתיקייה זמנית רק אם הוא לא מותקן במחשב
Source: "redist\SqlLocalDB.msi"; DestDir: "{tmp}"; Flags: deleteafterinstall; Check: not IsLocalDbInstalled

; כל הקבצים מתיקיית ה-publish, כולל תת-התיקייה Api
Source: "..\publish\TimeTracker\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
; הפעלה אוטומטית - אותו מפתח שמסך ההגדרות באפליקציה משתמש בו
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "TimeTracker"; ValueData: """{app}\{#MyAppExeName}"" --minimized"; Tasks: startup; Flags: uninsdeletevalue

[Run]
; התקנת LocalDB (מבקש הרשאות מנהל רק אם צריך להתקין)
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\SqlLocalDB.msi"" /qn IACCEPTSQLLOCALDBLICENSETERMS=YES"; Verb: "runas"; Flags: shellexec waituntilterminated; StatusMsg: "מתקין את SQL Server LocalDB..."; Check: not IsLocalDbInstalled

Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
; סוגרים את האפליקציה והשרת לפני הסרה
Filename: "{sys}\taskkill.exe"; Parameters: "/F /IM TimeTracker.Desktop.exe"; Flags: runhidden; RunOnceId: "KillDesktop"
Filename: "{sys}\taskkill.exe"; Parameters: "/F /IM TimeTracker.API.exe"; Flags: runhidden; RunOnceId: "KillApi"

[Code]
// בודק אם SQL Server LocalDB כבר מותקן במחשב
function IsLocalDbInstalled: Boolean;
var
  Versions: TArrayOfString;
begin
  Result := RegGetSubkeyNames(HKLM64,
    'SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions', Versions)
    and (GetArrayLength(Versions) > 0);
end;
