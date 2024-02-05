#!/bin/bash
set -e -x

cwd=$(pwd)

rm -rf fableBuild

dotnet fable src/MyLib.Core \
    -o fableBuild/mylib-core \
    --noCache \
    --fableLib fable-library

dotnet fable src/Client.JavaScript \
    --outDir fableBuild/Client \
    --noCache \
    --fableLib fable-library

dotnet fsi npm-adapter.fsx \
    fableBuild/Client \
    MyLib.Core=$cwd/fableBuild/mylib-core

node fableBuild/Client/Main.js
