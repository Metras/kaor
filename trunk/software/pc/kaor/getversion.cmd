@echo off

IF a%1 == a (
	echo Usage: getversion.cmd rootDir versionDir
	exit
)

IF a%2 == a (
	echo Usage: getversion.cmd rootDir versionDir
	exit
)

SET VERSION_TMP=%2\version_tmp.txt
SET VERSION_CS=%2\version.cs

svnversion %1 > %VERSION_TMP%

FOR /F %%i IN (%VERSION_TMP%) DO SET SVNVERSION=%%i
del %VERSION_TMP%

rem echo SVNVERSION=%SVNVERSION%

echo namespace kaor { > %VERSION_CS%
echo static partial class Program { >> %VERSION_CS%
echo static string revisionSVN = "%SVNVERSION%"; >> %VERSION_CS%
echo } } >> %VERSION_CS%
exit 0