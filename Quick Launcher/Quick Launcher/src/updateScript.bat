@ECHO OFF
PING 127.0.0.1 -n 3 >NUL
DEL Launcher.exe.orig /F /Q 2>NUL
MOVE temp\Launcher.temp.temp Launcher.temp 2>NUL
DEL temp /F /Q 2>NUL
REN Launcher.exe Launcher.exe.orig 2>NUL
REN Launcher.temp Launcher.exe 2>NUL
ECHO UPDATE SUCCESSFULLY!
PING 127.0.0.1 -n 2 >NUL
EXIT