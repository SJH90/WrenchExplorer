setlocal
set str=
set /p str=comment:
git add --all
git commit -a -m "%str% - %date% %time% by %USERNAME%"
git push

pause