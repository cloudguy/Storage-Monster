#!/bin/sh
cp ./Tools/xsp/* ./StorageMonster.Web/bin/
#line below not working, dunno why
#mono ./Tools/xsp/xsp4.exe --root StorageMonster.Web/ --applications /StorageMonster:./StorageMonster.Web
cd StorageMonster.Web
mono ../Tools/xsp/xsp4.exe  --applications /StorageMonster:./

