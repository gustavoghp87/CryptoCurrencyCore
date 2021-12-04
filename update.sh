#!bin/bash
home=$(pwd)

printf "*** pulling... *** "
git pull;
printf "*** stopping... *** "
docker stop crypto-ps;
printf "*** removing... *** "
docker rmi -f crypto-image;
printf "*** building... *** "
docker build -t crypto-image .;
printf "*** starting... *** "
docker run -p 8080:80 -d --name crypto-ps crypto-image;
printf "*** done *** "