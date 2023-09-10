#!/bin/sh
echo "Publishing:"
dotnet publish # > dev/null
echo "Deploying:"
set cwd = pwd
#
mv --force  ./usage/Floatingman.CommandLine.Application/bin/Debug/net7.0/publish/*.* ~/tmp/cmd # > dev/null
mv --force  ./usage/Floatingman.CommandLine.Command/bin/Debug/net7.0/publish/*.* ~/tmp/cmd/plugins/GenerateHexArray # > dev/null
#
cd ~/tmp/cmd
echo "Running: [Floatingman.CommandLine.Application GenerateHexArray -U 2 -V 2 --radius 10]"
echo "~"
./Floatingman.CommandLine.Application.exe GenerateHexArray -U 2 -V 2 --radius 10.0
echo "~"
cd $cwd
