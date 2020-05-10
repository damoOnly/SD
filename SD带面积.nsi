; �ýű�ʹ�� HM VNISEdit �ű��༭���򵼲���

; ��װ�����ʼ���峣��
!define PRODUCT_NAME "CTS6000"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "liutao"
!define PRODUCT_WEB_SITE "http://www.mycompany.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\WindowsFormsApplication1.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI �ִ����涨�� (1.67 �汾���ϼ���) ------
!include "MUI.nsh"

; MUI Ԥ���峣��
!define MUI_ABORTWARNING
!define MUI_ICON "SDApplication\CTS6000_64x64.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ���Э��ҳ��

; ���ѡ��ҳ��
!insertmacro MUI_PAGE_COMPONENTS
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES
; ��װ���ҳ��
!insertmacro MUI_PAGE_FINISH

; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_INSTFILES

; ��װ�����������������
!insertmacro MUI_LANGUAGE "SimpChinese"

; ��װԤ�ͷ��ļ�
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI �ִ����涨����� ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "SDSetup.exe"
InstallDir "$PROGRAMFILES\ɽ�������������װ����"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "Build\Debug\CommandManager.dll"
  File "Build\Debug\Dal.dll"
  File "Build\Debug\Entity.dll"
  File "Build\Debug\log4net.dll"
  File "Build\Debug\LogLib.dll"
  File "Build\Debug\LogLib.dll.config"
  File "Build\Debug\SDApplication.exe"
  File "Build\Debug\SDApplication.exe.config"
  
  File "Libs\DevExpress.XtraRichEdit.v14.2.dll"
  File "Libs\DevExpress.XtraPrinting.v14.2.dll"
  File "Libs\DevExpress.XtraLayout.v14.2.dll"
  File "Libs\DevExpress.XtraGrid.v14.2.dll"
  File "Libs\DevExpress.XtraEditors.v14.2.dll"
  File "Libs\DevExpress.XtraCharts.v14.2.Wizard.dll"
  File "Libs\DevExpress.XtraCharts.v14.2.UI.dll"
  File "Libs\DevExpress.XtraCharts.v14.2.dll"
  File "Libs\DevExpress.XtraBars.v14.2.dll"
  File "Libs\DevExpress.Utils.v14.2.dll"
  File "Libs\DevExpress.RichEdit.v14.2.Core.dll"
  File "Libs\DevExpress.Printing.v14.2.Core.dll"
  File "Libs\DevExpress.Office.v14.2.Core.dll"
  File "Libs\DevExpress.Data.v14.2.dll"
  File "Libs\DevExpress.Charts.v14.2.Core.dll"
  File "Libs\DevExpress.BonusSkins.v14.2.dll"
  File "Libs\ALARM1.WAV"
  File "Libs\SDData.db3"
  File "SDApplication\SystemConfig.xml"
  File "Libs\System.Data.SQLite.dll"
  File "Libs\CTS60�ж��к��������߼��ϵͳ�û�ʹ���ֲ�V1.0.1���򻯰棩.pdf"
  SetOutPath "$INSTDIR\x64"
  SetOverwrite ifnewer
  File "Libs\x64\SQLite.Interop.dll"
  SetOutPath "$INSTDIR\x86"
  SetOverwrite ifnewer
  File "Libs\x86\SQLite.Interop.dll"
  
  CreateDirectory "$SMPROGRAMS\ɽ�����������"
  CreateShortCut "$SMPROGRAMS\ɽ�����������\����Ũ�ȼ�����.lnk" "$INSTDIR\SDApplication.exe"
  CreateShortCut "$DESKTOP\����Ũ�ȼ�����.lnk" "$INSTDIR\SDApplication.exe"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\ɽ�����������\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\ɽ�����������\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\WADApplication.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\SDApplication.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#
Function GetNetFrameworkVersion
#--;��ȡ.Net Framework�汾֧��--#
Push $1
Push $0
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "InstallSuccess"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "2.0.50727.832"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.1.4322.573"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.0.3705.0"
StrCmp $0 1 KnowNetFrameworkVersion +1
StrCpy $1 "not .NetFramework"
KnowNetFrameworkVersion:
Pop $0
Exch $1
FunctionEnd

; �����������
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC01} ""
!insertmacro MUI_FUNCTION_DESCRIPTION_END

/******************************
 *  �����ǰ�װ�����ж�ز���  *
 ******************************/

Section Uninstall
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\SDApplication.exe"
  Delete "$INSTDIR\CommandManager.dll"
  Delete "$INSTDIR\Dal.dll"
  Delete "$INSTDIR\DataStruct.dll"
  Delete "$INSTDIR\Entity.dll"
  Delete "$INSTDIR\LibMessageHelper.dll"
  Delete "$INSTDIR\log4net.dll"
  Delete "$INSTDIR\LogLib.dll"
  Delete "$INSTDIR\LogLib.dll.config"
  Delete "$INSTDIR\WAD.accdb"
  Delete "$INSTDIR\ALARM1.WAV"
  Delete "$INSTDIR\SDApplication.exe"
  Delete "$INSTDIR\WADApplication.exe.Config"
  Delete "$INSTDIR\DevExpress.Charts.v11.2.Core.dll"
  Delete "$INSTDIR\DevExpress.Data.v11.2.dll"
  Delete "$INSTDIR\DevExpress.Data.v11.2.xml"
  Delete "$INSTDIR\DevExpress.Printing.v11.2.Core.dll"
  Delete "$INSTDIR\DevExpress.Printing.v11.2.Core.xml"
  Delete "$INSTDIR\DevExpress.Utils.v11.2.dll"
  Delete "$INSTDIR\DevExpress.Utils.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraBars.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraBars.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.UI.dll"
  Delete "$INSTDIR\DevExpress.XtraCharts.v11.2.UI.xml"
  Delete "$INSTDIR\DevExpress.XtraEditors.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraEditors.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraGrid.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraGrid.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraLayout.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraLayout.v11.2.xml"
  Delete "$INSTDIR\DevExpress.XtraRichEdit.v11.2.dll"
  Delete "$INSTDIR\DevExpress.XtraRichEdit.v11.2.xml"
  Delete "$INSTDIR\DevExpress.RichEdit.v11.2.Core.dll"
  Delete "$INSTDIR\WAD_SkinProject.dll"
  Delete "$INSTDIR\CTS60�ж��к��������߼��ϵͳ�û�ʹ���ֲ�V1.0.1���򻯰棩.pdf"
  Delete "$INSTDIR\CTS60�ж��к��������߼��ϵͳ�û�ʹ���ֲ�V1.0.2.pdf"
  Delete "$INSTDIR\SDApplication.exe.config"
  Delete "$INSTDIR\SDData.accdb"
  Delete "$INSTDIR\SDData.db3"
  Delete "$INSTDIR\SDSkinProject.dll"
  Delete "$INSTDIR\SQLite.Interop.dll"
  Delete "$INSTDIR\System.Data.SQLite.dll"
  Delete "$INSTDIR\SystemConfig.xml"
  Delete "$INSTDIR\Log\*.*"

  Delete "$SMPROGRAMS\ɽ�����������\Uninstall.lnk"
  Delete "$SMPROGRAMS\ɽ�����������\Website.lnk"
  Delete "$DESKTOP\����Ũ�ȼ�����.lnk"
  Delete "$SMPROGRAMS\ɽ�����������\����Ũ�ȼ�����.lnk"

  RMDir "$SMPROGRAMS\ɽ�����������"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "��ȷʵҪ��ȫ�Ƴ� $(^Name) ���������е������" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) �ѳɹ��ش����ļ�����Ƴ���"
FunctionEnd
