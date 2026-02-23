@echo off
git status --short > git_status_out.txt 2>&1
git branch >> git_status_out.txt 2>&1
git remote -v >> git_status_out.txt 2>&1
