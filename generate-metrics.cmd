set fxcopDir=%ProgramFiles%\Microsoft Visual Studio 10.0\Team Tools\Static Analysis Tools\FxCop\
set buildDir=%~dp0\build\Release\vsgraph
"%fxcopDir%\Metrics.exe" /file:%buildDir%\*.dll /file:%buildDir%\vsgraph.exe /out:%~dp0\build\vsgraph-metrics.xml /igc

pause
