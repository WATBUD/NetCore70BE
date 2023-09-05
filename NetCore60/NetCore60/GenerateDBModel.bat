@echo off
dotnet ef dbcontext scaffold "Server=127.0.0.1;Database=RNDatingDB;User=louis;Password=123456;" Pomelo.EntityFrameworkCore.MySql -o Models --force

pause
