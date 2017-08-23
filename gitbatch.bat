setlocal
set str=
set /p str=comment:
git rm -r --cached .
git add --all
git commit -a -m "%str% - %date% %time% by %USERNAME%"
git push

pause