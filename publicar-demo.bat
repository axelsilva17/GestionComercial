@echo off
REM ==============================================================
REM  GestionComercial — Script de Publicación (Demo)
REM  Genera una versión self-contained (no necesita .NET instalado)
REM ==============================================================

echo.
echo ========================================
echo  GestionComercial - Publicacion Demo
echo ========================================
echo.

SET PUBLISH_DIR=C:\GestionComercial_Demo
SET PROJECT=GestionComercial.UI

echo [1/4] Limpiando build anterior...
dotnet clean %PROJECT% -c Release --nologo -v q

echo [2/4] Publicando self-contained (Windows x64)...
dotnet publish %PROJECT% -c Release -r win-x64 --self-contained true ^
    -p:PublishSingleFile=false ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -p:EnableCompressionInSingleFile=true ^
    -o %PUBLISH_DIR% ^
    --nologo -v q

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Fallo la publicacion.
    pause
    exit /b 1
)

echo [3/4] Copiando archivos adicionales...
copy "Instalador\CredencialesDemo.txt" "%PUBLISH_DIR%\" >nul 2>&1

echo [4/4] Publicacion completada en: %PUBLISH_DIR%
echo.
echo El sistema esta listo para ser empaquetado como instalador.
echo.
pause
