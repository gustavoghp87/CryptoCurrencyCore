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

docker run -p 80:8080 -d --restart always -e domainName=http://190.231.194.136/ --name crypto-ps0 crypto-image;
docker run -p 8081:8080 -d --restart always -e domainName=http://190.231.194.136:8081/ --name crypto-ps1 crypto-image;
printf "*** done *** "
