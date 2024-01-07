
# 自动请求管理员权限
If (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    $arguments = "& '" + $myinvocation.mycommand.definition + "'"
    Start-Process powershell -Verb runAs -ArgumentList $arguments
    Break
}

cd F:\Projectlibrary\NetCoreBE\NetCore60\NetCore60
Stop-WebAppPool -Name "RNDatingService"
Stop-WebSite -Name "RNDatingService"

#Remove-Item -Path "F:\Projectlibrary\NetCoreBE\NetCore60\NetCore60\bin\Release\net6.0\publish\*" -Force -Recurse"

Start-Sleep -Seconds 5

dotnet publish -c Release -o "F:\Projectlibrary\NetCoreBE\NetCore60\NetCore60\bin\Release\net6.0\publish"

Start-WebAppPool -Name "RNDatingService"
Start-WebSite -Name "RNDatingService"
#pause
