#!/bin/bash

./minamo -cmd=build -config=./configs/vr_win32_oculus_dev.json -log=unity_vr_win32_oculus_dev.log
cat unity_vr_win32_oculus_dev.log | grep "MinamoLog"
cat unity_vr_win32_oculus_dev.log
