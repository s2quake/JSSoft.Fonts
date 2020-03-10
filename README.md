# 개요

FreeType 기반 폰트 텍스쳐 생성기

![main](./image01.png)

![preview](./image02.png)

# 도구

    Microsoft Visual Studio Community 2019

    .NET Framework 4.5

# 빌드 및 실행

    Window Key + S 누른후 검색창에서 Developer PowerShell For VS 2019 실행

    git clone https://github.com/s2quake/JSSoft.Font.git --recursive

    cd JSSoft.Font

    msbuild -t:restore 

    msbuild -t:build -p:configuration=Release

    .\bin\Release\jsfontApp.exe

