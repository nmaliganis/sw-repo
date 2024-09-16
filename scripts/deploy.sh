#!/bin/bash
ENV_ALIAS='dev'

API_ALIAS='sw.interprocess.api'

export AZ_RC_NAME_ID=sw.azurecr.io
export AZ_RC_ACCESS_USER_ID=sw
export AZ_RC_REGISTRY=sw
export AZ_RC_SECRET_PASS_KEY=Z+/555oBdA9PVsv0QN5Uy8Xm=uovn5Ok

echo "---------------------------------------"
echo "---------------------------------------"
echo "Clear Containers..."
docker rm -vf $(docker ps -a -q)
echo "Build images..."
docker-compose -f docker-compose.yml up -d

echo "---------------------------------------"
echo "---------------------------------------"
echo "Azure Arc Login..."
docker login $AZ_RC_NAME_ID --username $AZ_RC_ACCESS_USER_ID --password $AZ_RC_SECRET_PASS_KEY
echo "---------------------------------------"
echo "---------------------------------------"
echo "Tag Images"
docker tag sw/sw.interprocess:linux-latest $AZ_RC_NAME_ID/$AZ_RC_ACCESS_USER_ID/$API_ALIAS
docker push $AZ_RC_NAME_ID/$AZ_RC_ACCESS_USER_ID/$API_ALIAS
echo "Pushed ... $AZ_RC_NAME_ID/$AZ_RC_ACCESS_USER_ID/$API_ALIAS"

echo "---------------------------------------"
echo "---------------------------------------"
echo "Finished"