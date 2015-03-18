@echo off
set /p id=Enter wix:
"C:\PROGRA~1\WiX Toolset v3.9\bin\candle.exe" %id%.wxs
"C:\PROGRA~1\WiX Toolset v3.9\bin\light.exe" -ext WixUIExtension -ext WixUtilExtension %id%.wixobj
prompt

