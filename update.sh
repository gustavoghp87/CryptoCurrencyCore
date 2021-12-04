#!bin/bash
home=$(pwd)

printf "*** pulling... *** "
git pull;
printf "*** stopping... *** "
docker stop ***;
printf "*** removing... *** "
docker rm -f ***;         (image)
printf "*** building... *** "
docker build -t *** .;
printf "*** starting... *** "
docker run --name ***;
printf "*** done *** "