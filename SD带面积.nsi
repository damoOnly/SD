; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "CTS6000"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "liutao"
!define PRODUCT_WEB_SITE "http://www.mycompany.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\WindowsFormsApplication1.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "SDApplication\CTS6000_64x64.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 许可协议页面

; 组件选择页面
!insertmacro MUI_PAGE_COMPONENTS
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "SDSetup.exe"
InstallDir "$PROGRAMFILES\山盾气体检测软件安装程序"
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
  File "Libs\CTS60有毒有害气体在线监测系统用户使用手册V1.0.1（简化版）.pdf"
  SetOutPath "$INSTDIR\x64"
  SetOverwrite ifnewer
  File "Libs\x64\SQLite.Interop.dll"
  SetOutPath "$INSTDIR\x86"
  SetOverwrite ifnewer
  File "Libs\x86\SQLite.Interop.dll"
  
  CreateDirectory "$SMPROGRAMS\山盾气体检测软件"
  CreateShortCut "$SMPROGRAMS\山盾气体检测软件\气体浓度监测软件.lnk" "$INSTDIR\SDApplication.exe"
  CreateShortCut "$DESKTOP\气体浓度监测软件.lnk" "$INSTDIR\SDApplication.exe"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\山盾气体检测软件\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\山盾气体检测软件\Uninstall.lnk" "$INSTDIR\uninst.exe"
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

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#
Function GetNetFrameworkVersion
#--;获取.Net Framework版本支持--#
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

; 区段组件描述
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC01} ""
!insertmacro MUI_FUNCTION_DESCRIPTION_END

/******************************
 *  以下是安装程序的卸载部分  *
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
  Delete "$INSTDIR\CTS60有毒有害气体在线监测系统用户使用手册V1.0.1（简化版）.pdf"
  Delete "$INSTDIR\CTS60有毒有害气体在线监测系统用户使用手册V1.0.2.pdf"
  Delete "$INSTDIR\SDApplication.exe.config"
  Delete "$INSTDIR\SDData.accdb"
  Delete "$INSTDIR\SDData.db3"
  Delete "$INSTDIR\SDSkinProject.dll"
  Delete "$INSTDIR\SQLite.Interop.dll"
  Delete "$INSTDIR\System.Data.SQLite.dll"
  Delete "$INSTDIR\SystemConfig.xml"
  Delete "$INSTDIR\Log\*.*"

  Delete "$SMPROGRAMS\山盾气体检测软件\Uninstall.lnk"
  Delete "$SMPROGRAMS\山盾气体检测软件\Website.lnk"
  Delete "$DESKTOP\气体浓度监测软件.lnk"
  Delete "$SMPROGRAMS\山盾气体检测软件\气体浓度监测软件.lnk"

  RMDir "$SMPROGRAMS\山盾气体检测软件"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
